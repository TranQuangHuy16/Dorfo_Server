using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class ChatMessage
    {
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }
        public Guid? OrderId { get; set; }
        public string? MessageText { get; set; }
        public string? Attachments { get; set; }
        public DateTime CreatedAt { get; set; }

        public ChatConversation Conversation { get; set; } = null!;
        public User? FromUser { get; set; }
    }
}
