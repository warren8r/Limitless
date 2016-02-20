using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserCreationScreen.SQLInterfaces.Permissions
{
    public enum PageAccessValidation { ValidationError = -1, FailedValidation, PassedValidation }

    [System.Serializable]
    public class Permissions
    {
        // Public Constants
        public const byte CREATE_USER = (byte)1u << 0; // 0000 0001
        public const byte EDIT_USER = (byte)1u << 1; // 0000 0010
        public const byte CREATE_ROLE = (byte)1u << 2; // 0000 0100
        public const byte EDIT_ROLE = (byte)1u << 3; // 0000 1000
        public const byte ASSIGN_ROLE = (byte)1u << 4; // 0001 0000

        // Public Variables
        public byte actionPermissions = 0;
        public string role_name;
        public int usr_id;
        public List<PagePermission> pagePermissions = new List<PagePermission>();

        public int id
        {
            get { return _id; }
            set
            {
                if (_id < 0)
                    _id = value;
            }
        }

        public string first_name
        {
            get 
            {
                if (usr_id > -1)
                    return new SQLInterface().GetQuery("SELECT first_name FROM [TestDatabase].[dbo].[Users] WHERE usr_id=" + usr_id)[0, 0] as String;
                else
                    return "";
            }
        }

        public string last_name
        {
            get
            {
                if (usr_id > -1)
                    return new SQLInterface().GetQuery("SELECT last_name FROM [TestDatabase].[dbo].[Users] WHERE usr_id=" + usr_id)[0, 0] as String;
                else
                    return "";
            }
        }

        public string full_name
        {
            get
            {
                if (usr_id > -1)
                    return new SQLInterface().GetQuery("SELECT (first_name + ' ' + last_name) FROM [TestDatabase].[dbo].[Users] WHERE usr_id=" + usr_id)[0, 0] as String;
                else
                    return "";
            }
        }

        // Private Constants

        // Private Variables
        private int _id = -1;

        public bool canCreateUser
        {
            get { return (actionPermissions & CREATE_USER) > 0; }
            set
            {
                if (value)
                    actionPermissions = (byte)(actionPermissions | CREATE_USER);
                else
                    actionPermissions = (byte)(actionPermissions & (~CREATE_USER));
            }
        }

        public bool canEditUser
        {
            get { return (actionPermissions & EDIT_USER) > 0; }
            set
            {
                if (value)
                    actionPermissions = (byte)(actionPermissions | EDIT_USER);
                else
                    actionPermissions = (byte)(actionPermissions & (~EDIT_USER));
            }
        }

        public bool canCreateRole
        {
            get { return (actionPermissions & CREATE_ROLE) > 0; }
            set
            {
                if (value)
                    actionPermissions = (byte)(actionPermissions | CREATE_ROLE);
                else
                    actionPermissions = (byte)(actionPermissions & (~CREATE_ROLE));
            }
        }

        public bool canEditRole
        {
            get { return (actionPermissions & EDIT_ROLE) > 0; }
            set
            {
                if (value)
                    actionPermissions = (byte)(actionPermissions | EDIT_ROLE);
                else
                    actionPermissions = (byte)(actionPermissions & (~EDIT_ROLE));
            }
        }

        public bool canAssignRole
        {
            get { return (actionPermissions & ASSIGN_ROLE) > 0; }
            set
            {
                if (value)
                    actionPermissions = (byte)(actionPermissions | ASSIGN_ROLE);
                else
                    actionPermissions = (byte)(actionPermissions & (~ASSIGN_ROLE));
            }
        }

        // Private Constants

        // Private Variables

        public Permissions()
        {

        }

        public Permissions(byte startingPermissions)
        {
            actionPermissions = startingPermissions;
        }

        public Permissions(int role_id)
        {
            SetEquals(new SQLInterface().GetPermissionsQuery("SELECT * FROM TestDatabase.dbo.Roles WHERE role_id=" + role_id)[0]);
        }

        public static Permissions operator+(Permissions p1, Permissions p2)
        {
            Permissions toReturn = new Permissions();
            toReturn.SetEquals(p1);

            toReturn.actionPermissions |= p2.actionPermissions;

            if ((toReturn.pagePermissions.Count > 0) && (toReturn.pagePermissions.Count == p2.pagePermissions.Count))
            {
                for (int i = 0; i < toReturn.pagePermissions.Count; i++)
                {
                    if (toReturn.pagePermissions[i].pageName == p2.pagePermissions[i].pageName)
                    {
                        PagePermission pp = new PagePermission();
                        pp.pageName = toReturn.pagePermissions[i].pageName;
                        pp.canAccess = toReturn.pagePermissions[i].canAccess || p2.pagePermissions[i].canAccess;
                        toReturn.pagePermissions[i] = pp;
                    }
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Checks if an action is permitted using the allowed operations.
        /// </summary>
        /// <param name="operationPermissions">A byte representing what actions the user is trying to preform</param>
        /// <returns>Whether the user is allowed to do the action</returns>
        public bool CheckIfActionPermitted(byte operationPermissions, bool isAllPermissionsRequired = true)
        {
            if (isAllPermissionsRequired)
                return ((operationPermissions & actionPermissions) == operationPermissions);
            else
                return ((operationPermissions & actionPermissions) > 0);
        }

        /// <summary>
        /// Sets this Permissions equal to the given Permissions by value
        /// </summary>
        /// <param name="toSetTo">The Permissions to be set equal to</param>
        public void SetEquals(Permissions toSetTo)
        {
            _id = toSetTo.id;
            actionPermissions = toSetTo.actionPermissions;
            usr_id = toSetTo.usr_id;

            // Make sure that the page permissions carry over
            pagePermissions = new List<PagePermission>();

            for (int i = 0; i < toSetTo.pagePermissions.Count; i++)
            {
                pagePermissions.Add(toSetTo.pagePermissions[i]);
            }
        }

        public override string ToString()
        {
            string toReturn = "Permissions: ";

            for (int i = 0; i < pagePermissions.Count(); i++)
            {
                toReturn += pagePermissions[i].ToString();

                if (i != (pagePermissions.Count() - 1))
                {
                    toReturn += ", ";
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Sets p1 equalt to p2 by value
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public static void SetEquals(Permissions p1, Permissions p2)
        {
            p1.SetEquals(p2);
        }

        public static string ToString(Permissions p1)
        {
            return p1.ToString();
        }

        /// <summary>
        /// Return whether a specific user can access a page.
        /// </summary>
        /// <param name="pageName">The page to be checked</param>
        /// <param name="usr_id"></param>
        /// <returns>If the user can access a page, or an error if the specific usr_id doesn't exist</returns>
        public static PageAccessValidation CanAccessPage(string pageName, int usr_id, bool isInPersonalDb)
        {
            // Generate the usr_perms
            Permissions usr_perms = GetPermissionsFromUser(usr_id);

            return CanAccessPage(pageName, usr_perms, isInPersonalDb);
        }

        /// <summary>
        /// Return whether a specific user can access a page.
        /// </summary>
        /// <param name="pageName">The page to be checked</param>
        /// <param name="usr_perms">The permissions containing whether the user can enter or not</param>
        /// <returns>If the usr_perms can access a page, or an error if the specific page doesn't exist</returns>
        public static PageAccessValidation CanAccessPage(string pageName, Permissions usr_perms, bool isInPersonalDb)
        {
            if (isInPersonalDb)
                return PageAccessValidation.PassedValidation;
            else
            {
                PagePermission pp = usr_perms.pagePermissions.Find(x => x.pageName == pageName);

                if (pp != null)
                {
                    if (pp.canAccess)
                        return PageAccessValidation.PassedValidation;
                    else
                        return PageAccessValidation.FailedValidation;
                }
                else
                    return PageAccessValidation.ValidationError;
            }
        }

        /// <summary>
        /// Whether this permissions can access a specific page
        /// </summary>
        /// <param name="pageName">The page to be checked</param>
        /// <returns>If the usr_perms can access a page, or an error if the specific page doesn't exist</returns>
        public PageAccessValidation CanAccessPage(string pageName, bool isInPersonalDb)
        {
            return CanAccessPage(pageName, this, isInPersonalDb);
        }

        /// <summary>
        /// Get a Permissions object created from the information available to the user that relates to the given id.
        /// </summary>
        /// <param name="usr_id">The id of the user to get the permissions of</param>
        /// <returns>The permissions that the user has</returns>
        public static Permissions GetPermissionsFromUser(int usr_id)
        {
            // Generate the usr_perms
            Permissions usr_perms = new Permissions();
            Permissions[] perms = new SQLInterface().GetPermissionsQuery("EXEC TestDatabase.dbo.sp_GetRolesByUsr_id @usr_id=" + usr_id);

            if (perms.Count() <= 0)
            {
                return null;
            }

            usr_perms.SetEquals(perms[0]);

            for (int i = 0; i < perms.Count(); i++)
            {
                usr_perms += perms[i];
            }

            usr_perms.usr_id = usr_id;

            return usr_perms;
        }
    }

    public class PagePermission
    {
        public string pageName;
        public bool canAccess;

        public override string ToString()
        {
            return pageName + "=" + canAccess;
        }
    }
}