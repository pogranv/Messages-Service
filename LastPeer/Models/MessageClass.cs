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
    /// Класс сообщений.
    /// </summary>
    [DataContract]
    public class MessageClass
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public MessageClass() { }

        /// <summary>
        /// Конструктор, инициализирущий соответсвующие поля.
        /// </summary>
        /// <param name="subject">Тема сообщения.</param>
        /// <param name="message">Тело сообщения.</param>
        /// <param name="senderId">Почта отправителя.</param>
        /// <param name="receiverId">Почта получателя.</param>
        public MessageClass(string subject, string message, string senderId, string receiverId)
        {
            Subject = subject;
            Message = message;
            SenderId = senderId;
            ReceiverId = receiverId;
        }
        /// <summary>
        /// Тема сообщения.
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Почта отправителя.
        /// </summary>
        [Required]
        [DataMember]
        public string SenderId { get; set; }

        /// <summary>
        /// Почта получателя.
        /// </summary>
        [Required]
        [DataMember]
        public string ReceiverId { get; set; }
    }
}
