using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Providers.Entities;
using SS.Models;
using System.Web.Helpers;

namespace SS.Repository
{
    public class LocalRepository
    {

        #region Variables

        private SSSSContext entities = new SSSSContext();

        private const string MissingRole = "Role does not exist";
        private const string MissingUser = "User does not exist";
        private const string TooManyUser = "User already exists";
        private const string TooManyRole = "Role already exists";
        private const string AssignedRole = "Cannot delete a role with assigned users";

        #endregion

        #region Properties

        public int NumberOfUsers
        {
            get
            {
                return this.entities.User.Count();
            }
        }

        public int NumberOfRoles
        {
            get
            {
                return this.entities.Role.Count();
            }
        }

        #endregion

        #region Constructors

        public LocalRepository()
        {
            this.entities = new SSSSContext();
        }

        #endregion

        #region Query Methods

        public IQueryable<UserModel> GetAllUsers()
        {
            return from user in entities.User
                   orderby user.Email
                   select user;
        }

        public UserModel GetUser(int id)
        {
            return entities.User.SingleOrDefault(user => user.UserID == id);
        }

        public UserModel GetUser(string userName)
        {
            return entities.User.SingleOrDefault(user => user.Email == userName);
        }

        public IQueryable<UserModel> GetUsersForRole(string roleName)
        {
            return GetUsersForRole(GetRole(roleName));
        }

        public IQueryable<UserModel> GetUsersForRole(int id)
        {
            return GetUsersForRole(GetRole(id));
        }

        public IQueryable<UserModel> GetUsersForRole(RoleModel role)
        {
            if (!RoleExists(role))
                throw new ArgumentException(MissingRole);

            return from user in entities.User
                   where user.RoleID == role.RoleID
                   orderby user.Email
                   select user;
        }

        public IQueryable<RoleModel> GetAllUserRoles()
        {
            return from role in entities.Role
                   orderby role.Name
                   select role;
        }

        public RoleModel GetRole(int id)
        {
            return entities.Role.SingleOrDefault(role => role.RoleID == id);
        }

        public RoleModel GetRole(string name)
        {
            return entities.Role.SingleOrDefault(role => role.Name == name);
        }

        public RoleModel GetRoleForUser(string userName)
        {
            return GetRoleForUser(GetUser(userName));
        }

        public RoleModel GetRoleForUser(int id)
        {
            return GetRoleForUser(GetUser(id));
        }

        public RoleModel GetRoleForUser(UserModel user)
        {
            if (!UserExists(user))
                throw new ArgumentException(MissingUser);

            return user.Role;
        }

        #endregion

        #region Insert/Delete

        private void AddUser(UserModel user)
        {
            if (UserExists(user))
                throw new ArgumentException(TooManyUser);

            entities.User.Add(user); //.AddObject(user);
        }

        public void CreateUser(string fname, string lname, string password, string email, string roleName)
        {
            RoleModel role = GetRole(roleName);

            if (string.IsNullOrEmpty(fname.Trim()))
                throw new ArgumentException("The user name provided is invalid. Please check the value and try again.");
            if (string.IsNullOrEmpty(lname.Trim()))
                throw new ArgumentException("The name provided is invalid. Please check the value and try again.");
            if (string.IsNullOrEmpty(password.Trim()))
                throw new ArgumentException("The password provided is invalid. Please enter a valid password value.");
            if (string.IsNullOrEmpty(email.Trim()))
                throw new ArgumentException("The e-mail address provided is invalid. Please check the value and try again.");
            if (!RoleExists(role))
                throw new ArgumentException("The role selected for this user does not exist! Contact an administrator!");
            if (this.entities.User.Any(user => user.Email == email))
                throw new ArgumentException("Username already exists. Please enter a different user name.");

            UserModel newUser = new UserModel()
            {
                FirstName = fname,
                LastName = lname,
                Password = Crypto.HashPassword(password),
                Email = email,
                RoleID = role.RoleID
            };

            try
            {
                AddUser(newUser);
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The authentication provider returned an error. Please verify your entry and try again. " +
                    "If the problem persists, please contact your system administrator.");
            }

            // Immediately persist the user data
            Save();
        }

        //public void DeleteUser(UserModel user)
        //{
        //    if (!UserExists(user))
        //        throw new ArgumentException(MissingUser);

        //    entities.User.DeleteObject(user);
        //}

        //public void DeleteUser(string userName)
        //{
        //    DeleteUser(GetUser(userName));
        //}

        public void AddRole(RoleModel role)
        {
            if (RoleExists(role))
                throw new ArgumentException(TooManyRole);

            entities.Role.Add(role); //AddObject(role);
        }

        public void AddRole(string roleName)
        {
            RoleModel role = new RoleModel()
            {
                Name = roleName
            };

            AddRole(role);
        }

        //public void DeleteRole(RoleModel role)
        //{
        //    if (!RoleExists(role))
        //        throw new ArgumentException(MissingRole);

        //    if (GetUsersForRole(role).Count() > 0)
        //        throw new ArgumentException(AssignedRole);

        //    entities.Role.DeleteObject(role);
        //}

        //public void DeleteRole(string roleName)
        //{
        //    DeleteRole(GetRole(roleName));
        //}

        #endregion

        #region Persistence

        public void Save()
        {
            entities.SaveChanges();
        }

        #endregion

        #region Helper Methods

        public bool UserExists(UserModel user)
        {
            if (user == null)
                return false;

            return (entities.User.SingleOrDefault(u => u.UserID == user.UserID || u.Email == user.Email) != null);
        }

        public bool RoleExists(RoleModel role)
        {
            if (role == null)
                return false;

            return (entities.Role.SingleOrDefault(r => r.RoleID == role.RoleID || r.Name == role.Name) != null);
        }

        #endregion
    }
}