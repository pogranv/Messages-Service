using LastPeer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.IO;

namespace LastPeer.Controllers
{
    /// <summary>
    /// Класс контроллера для взаимодействия с пользователями и сообщениями.
    /// </summary>
    [Route("/api/[controller]")]
    public class Contorller : Controller
    {
        /// <summary>
        /// Статическое поле для рандомного генерирования.
        /// </summary>
        private static Random _rnd = new();
        /// <summary>
        /// Путь, по которому будет осуществляться сериализация пользователей.
        /// </summary>
        private static string usersPath = "Users.json";
        /// <summary>
        /// Путь, по которому будет осуществляться сериализация сообщений.
        /// </summary>
        private static string messagesPath = "Messages.json";

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        public List<User> Users { 
            get
            {
                return GetUsers().ToList();
            }
        }

        /// <summary>
        /// Получение списка сообщений.
        /// </summary>
        public List<MessageClass> Messages
        {
            get
            {
                return GetMessages().ToList();
            }
        }

        /// <summary>
        /// Генерация рандомного пользователя: его имени и почты.
        /// </summary>
        /// <returns>Возвращает сгенерированного пользователя.</returns>
        private User GetRandomUser()
        {
            int nameLength = _rnd.Next(3, 8);
            int emailLength = _rnd.Next(5, 10);
            string name = "";
            string email = "";
            for (int i = 0; i < nameLength; ++i)
            {
                name += (char)_rnd.Next('a', 'z' + 1);
            }
            for (int i = 0; i < emailLength; ++i)
            {
                email += (char)_rnd.Next('a', 'z' + 1);
            }
            email += "@email.com";
            return new User(name, email);
        }

        /// <summary>
        /// Генерация рандомного сообщения: его темы, текста отправителя и получателя.
        /// </summary>
        /// <param name="users">Список пользователей.</param>
        /// <returns>Возвращает сгенерированное сообщение.</returns>
        private MessageClass GetRandomMessage(List<User> users)
        {
            int lengthSubject = _rnd.Next(0, 10);
            int lenghtMessage = _rnd.Next(0, 20);
            string subject = "";
            string message = "";
            for (int i = 0; i < lengthSubject; ++i)
            {
                subject += (char)_rnd.Next('A', 'z' + 1);
            }
            for (int i = 0; i < lengthSubject; ++i)
            {
                message += (char)_rnd.Next('A', 'z' + 1);
            }
            int from = _rnd.Next(0, users.Count);
            int to = _rnd.Next(0, users.Count);
            return new MessageClass(subject, message, users[from].Email, users[to].Email);
        }


        /// <summary>
        /// Осуществляет десериализацию списка пользователей.
        /// </summary>
        /// <returns>Возвращает десериализованный список пользователей.</returns>
        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            try
            {
                using var fs = new FileStream(usersPath, FileMode.Open, FileAccess.Read);
                var formatter = new DataContractJsonSerializer(typeof(List<User>));
                return (List<User>)formatter.ReadObject(fs);
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        /// <summary>
        /// Осуществляет десериализацию списка сообщений.
        /// </summary>
        /// <returns>Возвращает десериализованный список сообщений.</returns>
        [HttpGet("GetMessages")]
        public IEnumerable<MessageClass> GetMessages()
        {
            try
            {
                using var fs = new FileStream(messagesPath, FileMode.Open, FileAccess.Read);
                var formatter = new DataContractJsonSerializer(typeof(List<MessageClass>));
                return (List<MessageClass>)formatter.ReadObject(fs);
            }
            catch (Exception)
            {
                return new List<MessageClass>();
            }
        }

        /// <summary>
        /// Осуществляет получение пользователя по его почте.
        /// </summary>
        /// <param name="email">Почта пользователя, которого необходимо вернуть.</param>
        /// <returns>Возвращает код возврата и соответсвующее тело или сообщение.</returns>
        [HttpGet("GetUserByEmail{email}")]
        public IActionResult GetByEmail(string email)
        {
            var user = Users.SingleOrDefault(p => p.Email == email);

            if (user == null)
            {
                return NotFound("Пользователь с такой почтой не найден.");
            }

            return Ok(user);
        }

        /// <summary>
        /// Осуществляет получение сообщения по почте отправителя и получателя.
        /// </summary>
        /// <param name="senderId">Почта отправителя.</param>
        /// <param name="receiverId">Почта получателя.</param>
        /// <returns>Возвращает код возврата и соответсвующее тело.</returns>
        [HttpGet("GetMessagesBySenderIdAndReceivedId{senderId}, {receiverId}")]
        public IActionResult GetMessageBySenderAndReceived(string senderId, string receiverId)
        {
            var filterMessages = Messages.Where(m => m.SenderId == senderId && m.ReceiverId == receiverId).ToList();

            return Ok(filterMessages);
        }

        /// <summary>
        /// Осуществляет получение сообщения по почте отправителя.
        /// </summary>
        /// <param name="senderId">Почта отправителя.</param>
        /// <returns>Возвращает код возврата и соответсвующее тело.</returns>

        [HttpGet("GetBySenderId{senderId}")]
        public IActionResult GetBySender(string senderId)
        {
            var filterMessages = Messages.Where(m => m.SenderId == senderId).ToList();

            return Ok(filterMessages);
        }

        /// <summary>
        /// Осуществляет получение сообщения по почте получателя.
        /// </summary>
        /// <param name="receivedId">Почта получателя.</param>
        /// <returns>Возвращает код возврата и соответсвующее тело.</returns>
        [HttpGet("GetByReceivedId{receivedId}")]
        public IActionResult GetByreceived(string receivedId)
        {
            var filterMessages = Messages.Where(m => m.ReceiverId == receivedId).ToList();

            return Ok(filterMessages);
        }

        /// <summary>
        /// Создает случайный набор пользователей и их сообщения.
        /// </summary>
        /// <returns>Возвращает код возврата и соответсвующее тело.</returns>
        [HttpPost("MakeUsersAndMessages")]
        public IActionResult Post()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InitUsers();
            InitMessages();

            return Ok();
        }

        /// <summary>
        /// Осуществляет регистрацию нового пользователя по имени и почте.
        /// </summary>
        /// <param name="user">Пользователь, которого необходимо зарегистрировать.</param>
        /// <returns>Возвращает код возврата и соответсвующее сообщение.</returns>
        [HttpPost("RegisterUser")]
        public IActionResult Post(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (Users.Select(user => user.Email).Contains(user.Email))
            {
                return BadRequest("Пользователь с такой почтой уже зарегистрирован.");
            }
            AddUser(user);
            return Ok();
        }

        /// <summary>
        /// Добавляет пользователя в сериализованный файл.
        /// </summary>
        /// <param name="user">Пользователь, которого нужно добавить.</param>
        private void AddUser(User user)
        {
            var users = Users;
            users.Add(user);
            users = users.OrderBy(user => user.Email).ToList();

            DataContractJsonSerializer format =
            new DataContractJsonSerializer(typeof(List<User>));

            try
            {
                using (FileStream fileStream = new FileStream(usersPath, FileMode.Create))
                {
                    format.WriteObject(fileStream, users);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Генерирует случайный набор сообщений и сериализует его в json файл.
        /// </summary>
        private void InitMessages()
        {
            int countMessages = _rnd.Next(5, 20);
            var messages = new List<MessageClass>();
            HashSet<string> hashSet = new();
            for (int i = 0; i < countMessages; ++i)
            {
                messages.Add(GetRandomMessage(Users));
            }

            messages = messages.OrderBy(message => message.SenderId).ToList();

            DataContractJsonSerializer format =
            new DataContractJsonSerializer(typeof(List<MessageClass>));

            try
            {
                using (FileStream fileStream = new FileStream(messagesPath, FileMode.Create))
                {
                    format.WriteObject(fileStream, messages);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Генерирует случайный набор пользователей и сериализует его в json файл.
        /// </summary>
        private void InitUsers()
        {
            int countUsers = _rnd.Next(3, 9);
            List<User> users = new();
            HashSet<string> hashSet = new();
            while (users.Count < countUsers)
            {
                User newUser = GetRandomUser();
                if (hashSet.Contains(newUser.Email))
                    continue;

                users.Add(newUser);
                hashSet.Add(newUser.Email);
            }

            users = users.OrderBy(user => user.Email).ToList();

            DataContractJsonSerializer format =
            new DataContractJsonSerializer(typeof(List<User>));

            try
            {
                using (FileStream fileStream = new FileStream(usersPath, FileMode.Create))
                {
                    format.WriteObject(fileStream, users);
                }
            } 
            catch (Exception) { }
        }
    }
}
