using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;
using SS.Models;

namespace SS
{

    public class MyRoleProvider : RoleProvider
    {
        SSSSContext db = new SSSSContext();
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        #region not using these abstract method but need to have it :Rabin
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        #endregion

        public override string[] GetRolesForUser(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException();
            }
            else if (email == string.Empty)
            {
                throw new ArgumentException();
            }
            else
            {
                UserModel user = db.User.Single(x => x.Email == email);
                string[] roles = new string[1];
                if (user.Role == null)
                {
                    return roles;
                }
                else
                {
                    roles[0] = user.Role.Name;
                    return roles;
                }
            }
           
            //List<string> roles = new List<string>();

            //using (UATContext _db = new UATContext())
            //{
            //    try
            //    {
            //        var dbRoles = from r in _db.Role
            //                      where r.= email
            //                      select r;
 
            //        foreach (var role in dbRoles)
            //        {
            //            roles.Add(role.Role.RoleName);
            //        }
 
            //    }
            //    catch
            //    {
            //    }
            //}
 
            //return roles.ToArray();
            //throw new NotImplementedException();
        }

        #region don't need this method :Rabin
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override bool IsUserInRole(string email, string roleName)
        {
            if (!db.User.Any(x => x.Email == email) || !db.Role.Any(x => x.Name == roleName))
            {
                return false;
                throw new ProviderException();
            }
            else if (email == "" || roleName == "")
            {
                return false;
                throw new ArgumentException();
            }
            else if (email == null || roleName == null)
            {
                return false;
                throw new ArgumentNullException();
            }
            else
            {
                UserModel user = db.User.Single(x => x.Email == email);

                if (user.Role.Name == roleName)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //using (SSSSContext db = new SSSSContext())
            //{
            //    UserModel user = db.User.FirstOrDefault(u => u.Email == email);

            //    var roles = db.Role.Select(r => r.RoleID == user.RoleID);
            //    //var roles = from ur in user.Role.ToString()
            //    //            from r in db.Role
            //    //            where ur == r.RoleID
            //    //            select r.Name;
            //    if (user != null)
            //        return roles.Any(r => r.Email);
            //    else
            //        return false;
            //}
            //throw new NotImplementedException();
        }

        #region again, no need of this methods: Rabin
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
 
    }
}