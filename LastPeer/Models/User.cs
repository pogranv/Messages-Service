using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace LastPeer.Models
{
    /// <summary>
    /// Класс пользователя.
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public User() { }

        /// <summary>
        /// Конструктор, инициализирущий соответсвующие поля.
        /// </summary>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="email">Почта пользователя.</param>
        public User(string name, string email)
        {
            UserName = name;
            Email = email;
        }
        
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Почта пользователя.
        /// </summary>
        [Required]
        [DataMember]
        public string Email { get; set; }
    }

}
