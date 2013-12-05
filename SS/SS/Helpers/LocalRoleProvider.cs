using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SS.Models;
using SS.Repository;

namespace SS.Helpers
{
    public class LocalRoleProvider :RoleProvider
    {
        #region added
        private LocalRepository repository;

        public LocalRoleProvider()
        {
            this.repository = new LocalRepository();
        }
        #endregion

        public override bool IsUserInRole(string username, string roleName)
        {
            UserModel user = repository.GetUser(username);
            RoleModel role = repository.GetRole(roleName);

            if (!repository.UserExists(user))
                return false;
            if (!repository.RoleExists(role))
                return false;

            return user.Role.Name == role.Name;
            //throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            RoleModel role = this.repository.GetRoleForUser(username);
            if (!this.repository.RoleExists(role))
                return new string[] { string.Empty };

            return new string[] { role.Name };
            //throw new NotImplementedException();
        }

        #region notimplemented methods

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

       

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        

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