using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using UserCreationScreen.SQLInterfaces;

namespace UserCreationScreen.SQLInterfaces.Storage
{
    public class UserData
    {
        // Public Constants

        // Public Variables
        public string firstName;
        public string lastName;
        public string address1;
        public string address2;
        public string address3;
        public string city;
        public string state;
        public string zipCode;
        public string country;

        public string phoneNumber
        {
            get { return _phoneNumber; }
            set 
            {
                if (ValidatePhoneNumber(value))
                    _phoneNumber = value;
                else
                    _phoneNumber = null;
            }
        }

        public string emailAddress
        {
            get { return _emailAddress; }
            set 
            {
                if (ValidateEmailAddress(value))
                    _emailAddress = value;
                else
                    _emailAddress = null;
            }
        }

        // This variable should only be set on creation of an instance of UserData
        // and should only have such done when querying from the server
        public int id
        {
            get { return _id; }
            set
            {
                if (_id < 0)
                {
                    _id = value;
                }
            }
        }

        // Private Constants

        // Private Variables
        private string _phoneNumber;
        private string _emailAddress;
        private int _id = -1;

        public UserData()
        {

        }

        /// <summary>
        /// Constructor: Takes the given id number and gets all the nessicary values from the table Users
        /// </summary>
        /// <param name="id_number">The id number of the user</param>
        public UserData(int id_number, bool isInPersonalDB, int userID)
        {
            SQLInterface intrfc = new SQLInterface();
            id = id_number;

            try
            {
                if (!isInPersonalDB)
                {
                    UserData usrFrom = intrfc.GetUserDataQuery("SELECT * FROM dbo.Users WHERE usr_id=" + id)[0];
                    this.SetEquals(usrFrom);
                }
                else
                {
                    UserData usrFrom = intrfc.GetUserDataQuery("SELECT * FROM TestDatabase2.dbo.Users" + userID + " WHERE usr_id=" + id)[0];
                    this.SetEquals(usrFrom);
                }
            }
            catch (Exception ex){}
        }

        /// <summary>
        /// Converts the UserData to a string (formatted to be displayed as a list element)
        /// </summary>
        /// <returns>The string version</returns>
        public override string ToString()
        {
            string toReturn = "";

            toReturn += id.ToString() + " | " +
                        firstName + " " + lastName + 
                        " Phone: " + phoneNumber +
                        " Email: " + emailAddress +
                        " Address: " + address1;

            if (address2 != null)
                toReturn += ", " + address2;

            if (address3 != null)
                toReturn += ", " + address3;

            toReturn += city + " " + 
                state + ", " + 
                zipCode + ", " + 
                country;

            return toReturn;
        }

        /// <summary>
        /// Sets this UserData equal in values to userFrom
        /// </summary>
        /// <param name="userFrom">The UserData that this UserData will be set to</param>
        public void SetEquals(UserData userFrom)
        {
            _id = userFrom.id;
            firstName = userFrom.firstName;
            lastName = userFrom.lastName;
            phoneNumber = userFrom.phoneNumber;
            emailAddress = userFrom.emailAddress;
            address1 = userFrom.address1;
            address2 = userFrom.address2;
            address3 = userFrom.address3;
            city = userFrom.city;
            state = userFrom.state;
            zipCode = userFrom.zipCode;
            country = userFrom.country;
        }

        /// <summary>
        /// Checks if this user is functionally the same as the other user
        /// </summary>
        /// <param name="userFrom">The UserData to be check against</param>
        /// <returns>Whether they are equal, with equal being true</returns>
        public bool GetEquals(UserData userFrom)
        {
            return (_id == userFrom.id) &&
            (firstName == userFrom.firstName) &&
            (lastName == userFrom.lastName) &&
            (phoneNumber == userFrom.phoneNumber) &&
            (emailAddress == userFrom.emailAddress) &&
            (address1 == userFrom.address1) &&
            (address2 == userFrom.address2) &&
            (address3 == userFrom.address3) &&
            (city == userFrom.city) &&
            (state == userFrom.state) &&
            (zipCode == userFrom.zipCode) &&
            (country == userFrom.country);
        }

        /// <summary>
        /// Gets the string to delete a record with the matching id
        /// </summary>
        /// <param name="toDeleteId">The usr_id of the record to be deleted</param>
        /// <returns>The string that when executed will delete the user</returns>
        public static string DeleteRecordString(int toDeleteId, bool isInPersonalDatabase, int userID)
        {
            string toReturn = "";

            if (!isInPersonalDatabase)
                toReturn += "DELETE FROM dbo.Users" +
                    " WHERE usr_id=" + toDeleteId;
            else
                toReturn += "DELETE FROM TestDatabase2.dbo.Users" + userID +
                        " WHERE usr_id=" + toDeleteId;

            return toReturn;
        }

        /// <summary>
        /// Validate a phone number
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate</param>
        /// <param name="setValue">Whether to set the internal phone number</param>
        /// <returns>Whether the phone number is valid or not</returns>
        public bool ValidatePhoneNumber(string phoneNumber, bool setValue = false)
        {
            if (new Regex(@"\(\d{3}\)\d{3}-\d{4}").Match(phoneNumber).Success)
            {
                if (setValue) _phoneNumber = phoneNumber;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Validate an email address
        /// </summary>
        /// <param name="emailAddress">The email address to validate</param>
        /// <param name="setValue">Whether to set the internal email address</param>
        /// <returns>Whether the email address is valid or not</returns>
        public bool ValidateEmailAddress(string emailAddress, bool setValue = false)
        {
            if (new Regex(@".+@[\w\d]+\.[\w\d]+").Match(emailAddress).Success)
            {
                if (setValue) _emailAddress = emailAddress;
                return true;
            }
            else
                return false;
        }

        public string[] AddToUsersTableString(bool isInPersonalDatabase, int userID)
        {
            // Calculate the insert command and the size of the returned array
            int size = 10;         
            string insertCommand;

            if (isInPersonalDatabase)
                insertCommand = "INSERT INTO TestDatabase2.dbo.Users" + userID + "(first_name, last_name, address1,";
            else
                insertCommand = "INSERT INTO dbo.Users(first_name, last_name, address1,";

            if ((address2 != null) && (address2 != ""))
            {
                size++;
                insertCommand += " address2,";
            }

            if ((address3 != null) && (address3 != ""))
            {
                size++;
                insertCommand += " address3,";
            }

            insertCommand += " phone_number, email_address, city, state, zip, country) VALUES (";

            // Create the string array and set initial values
            string[] toReturn = new string[size];
            toReturn[0] = insertCommand;

            // Add paramaters and validate that all nessicary values are assigned
            int pos = 0;
            if ((firstName != null) && (firstName != ""))
            {
                toReturn[0] += "@" + pos + ",";
                pos++;
                toReturn[pos] = firstName;
            }

            if ((lastName != null) && (lastName != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = lastName;
            }

            if ((address1 != null) && (address1 != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = address1;
            }

            if ((address2 != null) && (address2 != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = address2;
            }

            if ((address3 != null) && (address3 != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = address3;
            }

            if ((phoneNumber != null) && (phoneNumber != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = phoneNumber;
            }

            if ((emailAddress != null) && (emailAddress != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = emailAddress;
            }

            if ((city != null) && (city != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = city;
            }

            if ((state != null) && (state != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = state;
            }

            if ((zipCode != null) && (zipCode != ""))
            {
                toReturn[0] += " @" + pos + ",";
                pos++;
                toReturn[pos] = zipCode;
            }

            if ((country != null) && (country != ""))
            {
                toReturn[0] += " @" + pos;
                pos++;
                toReturn[pos] = country;
            }

            toReturn[0] += ")";
            return toReturn;
        }

        public string[] UpdateUserTableString(bool isInPersonalDatabase, int userID)
        {
            // Initialize toReturn and add the command string
            string[] toReturn = new string[12];
            toReturn[0] = "UPDATE ";

            if (isInPersonalDatabase)
                toReturn[0] += "TestDatabase2.dbo.Users" + userID;
            else
                toReturn[0] += "dbo.Users";
            
            toReturn[0] += " SET first_name=@0, last_name=@1, address1=@2, address2=@3, address3=@4, phone_number=@5, email_address=@6, city=@7, state=@8, zip=@9, country=@10 WHERE usr_id=" + id;

            // Add paramaters and validate that all nessicary values are assigned
            int pos = 0;
            if ((firstName != null) && (firstName != ""))
            {
                pos++;
                toReturn[pos] = firstName;
            }

            if ((lastName != null) && (lastName != ""))
            {
                pos++;
                toReturn[pos] = lastName;
            }

            if ((address1 != null) && (address1 != ""))
            {
                pos++;
                toReturn[pos] = address1;
            }

            if ((address2 != null) && (address2 != ""))
            {
                pos++;
                toReturn[pos] = address2;
            }
            else
            {
                pos++;
                toReturn[pos] = null;
            }

            if ((address3 != null) && (address3 != ""))
            {
                pos++;
                toReturn[pos] = address3;
            }
            else
            {
                pos++;
                toReturn[pos] = null;
            }

            if ((phoneNumber != null) && (phoneNumber != ""))
            {
                pos++;
                toReturn[pos] = phoneNumber;
            }

            if ((emailAddress != null) && (emailAddress != ""))
            {
                pos++;
                toReturn[pos] = emailAddress;
            }

            if ((city != null) && (city != ""))
            {
                pos++;
                toReturn[pos] = city;
            }

            if ((state != null) && (state != ""))
            {
                pos++;
                toReturn[pos] = state;
            }

            if ((zipCode != null) && (zipCode != ""))
            {
                pos++;
                toReturn[pos] = zipCode;
            }

            if ((country != null) && (country != ""))
            {
                pos++;
                toReturn[pos] = country;
            }

            return toReturn;
        }
    }
}