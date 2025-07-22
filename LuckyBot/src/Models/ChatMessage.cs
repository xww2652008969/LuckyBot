using LuckyBot.Models.constants;
using Newtonsoft.Json;

namespace LuckyBot.Models
{
    public class ChatMessage(long groupId, long userId)
    {
        [JsonProperty("group_id")]
        public long GroupId { get; set; } = groupId;

        [JsonProperty("user_id")]
        public long UserId { get; set; } = userId;

        [JsonProperty("message")]
        private List<ChatMessageData> Message { get; set; } = [];

        private ChatMessage AddMessage(string msgType, MessagePayload data)
        {
            Message.Add(new ChatMessageData(msgType, data));
            return this;
        }

        public ChatMessage AddImage(string data)
        {
            var d = new MessagePayload { File = data };
            return AddMessage(MessageType.MsgTypeImage, d);
        }

        public ChatMessage AddFace(int id)
        {
            var payload = new MessagePayload { Id = id.ToString() };
            return AddMessage(MessageType.MsgTypeFace, payload);
        }

        public ChatMessage AddRecord(string url)
        {
            var payload = new MessagePayload { File = url };
            return AddMessage(MessageType.MsgTypeRecord, payload);
        }

        public ChatMessage AddVideo(string url)
        {
            var payload = new MessagePayload { File = url };
            return AddMessage(MessageType.MsgTypeVideo, payload);
        }

        public ChatMessage AddReply(long id)
        {
            var payload = new MessagePayload { Id = id.ToString() };
            return AddMessage(MessageType.MsgTypeReply, payload);
        }

        public ChatMessage AddMusicCard(string t, int id)
        {
            var payload = new MessagePayload { Type = t, Id = id.ToString() };
            return AddMessage(MessageType.MsgTypeMusic, payload);
        }

        public ChatMessage AddFile(string url, string name)
        {
            var payload = new MessagePayload { File = url, Name = name };
            return AddMessage(MessageType.MsgTypeFile, payload);
        }


        public ChatMessage AddText(string text, bool isSpace = true, bool isLineBreak = false)
        {
            if (isSpace)
            {
                text = text + " ";
            }

            if (isLineBreak)
            {
                text += "\n";
            }

            var payload = new MessagePayload { Text = text };
            return AddMessage(MessageType.MsgTypeText, payload);
        }

        public ChatMessage AddAt(long qq)
        {
            var payload = new MessagePayload { Qq = qq == 0 ? "all" : qq.ToString() };
            return AddMessage(MessageType.MsgTypeAt, payload).AddText(" ", false, false);
        }

        public class ChatMessageData(string type, MessagePayload data)
        {
            [JsonProperty("type")]
            public string Type { get; set; } = type;

            [JsonProperty("data")]
            public MessagePayload Data { get; set; } = data;
        }

        public class MessagePayload
        {
            [JsonProperty("qq")]
            public string? Qq { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("text")]
            public string? Text { get; set; }

            [JsonProperty("file")]
            public string? File { get; set; }

            [JsonProperty("id")]
            public string? Id { get; set; }

            [JsonProperty("url")]
            public string? Url { get; set; }

            [JsonProperty("sub_Type")]
            public int? SubType { get; set; }

            [JsonProperty("file_Size")]
            public string? FileSize { get; set; }

            [JsonProperty("type")]
            public string? Type { get; set; }

            [JsonProperty("data")]
            public string? Data { get; set; }

            [JsonProperty("content")]
            public List<Message>? Content { get; set; }
        }
    }
}
