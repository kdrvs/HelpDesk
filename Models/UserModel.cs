using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Post { get; set; }
        public string Password { get; set; }
        public string ConfirmPass { get; set; }

        public string errorMessage;
        public string ServerRole { get; set; }

        private Users user;
        public Users User
        {
            get { return user; }
            set { user = value; }
        }

        public UserModel()
        {
            
        }

        public UserModel(HttpContext context)
        {
            var claims = context.User.Identity as ClaimsIdentity;
            int _userId = int.Parse(claims.Claims.ElementAt(0).Value);
            user = findUserFromDb(_userId);

        }

        public UserModel(string id)
        {
            int userId = 0;
            int.TryParse(id, out userId);
            user = findUserFromDb(userId);
        }

        public bool isValid()
        {
            bool valid = false;
            using (HelpdeskContext db = new HelpdeskContext())
            {
                try
                {
                    user = db.Users.Where(u => u.Email == Email).Single();

                    if (User != null)
                    {
                        Authentication auth = db.Authentication.Find(User.Id);
                        if (auth != null && auth.UserPassword == Authentication.hashed(Password))
                            valid = true;                        
                    }
                }
                catch { }
                
            }

            return valid;
        }

        private Users findUserFromDb(int userId)
        {
            Users _user = null;
            using(HelpdeskContext db = new HelpdeskContext())
            {
                _user = db.Users.Find(userId);
            }
            return _user;
        }

        public int changePass(out string message)
        {
            message = "";
            if (Password == null)
            {
                message = "Введите новый пароль";
                return -1;
            }
                
            if (ConfirmPass == null)
            {
                message = "Повторите пароль";
                return -1;
            }
                
            if (Password != ConfirmPass)
            {
                message = "Введённые пароли не совпадают";
                return -1;
            }                

            string pass = Authentication.hashed(Password);
            string confirm = Authentication.hashed(ConfirmPass);
            int result = -1;
            if(pass == confirm)
            {
                try
                {
                    using (HelpdeskContext db = new HelpdeskContext())
                    {
                        Authentication auth = db.Authentication.Find(user.Id);
                        auth.UserPassword = pass;
                        db.SaveChanges();
                        message = "Пароль успешно изменён";
                        result = user.Id;
                    }
                }
                catch(Exception e)
                {
                    message = e.Message;
                    result = -1;
                }
            }

            return result;           

        }

        public static List<ServerRoles> getRolesFromDb()
        {
            List<ServerRoles> roles = new List<ServerRoles>();
            using(HelpdeskContext db = new HelpdeskContext())
            {
                roles = db.ServerRoles.ToList();
            }
            return roles;
        }

        public int saveChangeToDb(out string message)
        {
            message = "";
            int result = -1;
            if (Name == null
                || Surname == null
                || Email == null
                || Post == null
                || ServerRole == null
                )
            {
                message = "Заполните данные пользователя";
                return -1;
            }

            if(Password != null && ConfirmPass == null)
            {
                message = "Пароли не совпадают";
                return -1;
            }

            if (Password == null && ConfirmPass != null)
            {
                message = "Пароли не совпадают";
                return -1;
            }

            if (Password != null && Password != ConfirmPass)
            {
                message = "Пароли не совпадают";
                return -1;
            }
                
            try
            {
                using(HelpdeskContext db = new HelpdeskContext())
                {
                    Users _user = db.Users.Include(u => u.Authentication).Where(u => u.Id == UserId).FirstOrDefault();
                    if (_user.Name != Name)
                        _user.Name = Name;
                    if (_user.Surname != Surname)
                        _user.Surname = Surname;
                    if (_user.Email != Email)
                        _user.Email = Email;
                    if (_user.Post != Post)
                        _user.Post = Post;
                    if (_user.ServerRole != ServerRole)
                        _user.ServerRole = ServerRole;
                    if(Password != null && ConfirmPass != null && Password == ConfirmPass)
                    {
                        var auth = db.Authentication.Where(a => a.IdUser == _user.Id).FirstOrDefault();
                        auth.UserPassword = Authentication.hashed(Password);
                    }

                    db.SaveChanges();
                    result = _user.Id;
                    message = "Данные изменены";
                }
            }
            catch(Exception e)
            {
                message = e.Message;
            }

            return result;
        }

        public int saveNewUser()
        {
            if (Name == null
                || Surname == null
                || Email == null
                || Post == null
                || ServerRole == null
                || Password == null
                || ConfirmPass == null
                )
            {
                errorMessage = "Заполнены не все данные";
                return -1;
            }
            if (Password != ConfirmPass)
            {
                errorMessage = "Пароли не совпадают";
                return -1;
            }
            int result = -1;

            try
            {
                using (HelpdeskContext db = new HelpdeskContext())
                {
                    if(db.Users.Select(u=>u.Email).Contains(Email))
                    {
                        errorMessage = "Такой Email уже зарегистрирован";
                        return -1;
                    }
                    Users newUser = new Users()
                    {
                        Email = this.Email,
                        Name = this.Name,
                        Surname = this.Surname,
                        Post = this.Post,
                        ServerRole = this.ServerRole
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    int userId = db.Users
                        .Where(u => u.Name == this.Name && u.Surname == this.Surname && u.Post == this.Post)
                        .LastOrDefault()
                        .Id;
                    var pass = new Authentication()
                    {
                        IdUser = userId,
                        UserPassword = Authentication.hashed(this.Password)
                    };
                    db.Authentication.Add(pass);
                    db.SaveChanges();
                    result = userId;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return result;
        }

    }
}
