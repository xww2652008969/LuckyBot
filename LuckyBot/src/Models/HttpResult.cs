using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace LuckyBot.Models;

public class HttpResult
{
    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("retcode")]
    public int RetCode { get; set; }

    [JsonProperty("data")]
    public ResultData? Data { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("wording")]
    public string? Wording { get; set; }

    [JsonProperty("echo")]
    public object? Echo { get; set; }
}

public class ResultData
{
    [JsonProperty("group_id")]
    public long? GroupId { get; set; }

    [JsonProperty("user_id")]
    public long? UserId { get; set; }

    [JsonProperty("nickname")]
    public string Nickname { get; set; }

    [JsonProperty("card")]
    public string Card { get; set; }

    [JsonProperty("sex")]
    public string Sex { get; set; }

    [JsonProperty("age")]
    public int? Age { get; set; }

    [JsonProperty("area")]
    public string Area { get; set; }

    [JsonProperty("level")]
    public string Level { get; set; }

    [JsonProperty("qq_level")]
    public int? QqLevel { get; set; }

    [JsonProperty("join_time")]
    public long? JoinTime { get; set; }

    [JsonProperty("last_sent_time")]
    public long? LastSentTime { get; set; }

    [JsonProperty("title_expire_time")]
    public long? TitleExpireTime { get; set; }

    [JsonProperty("unfriendly")]
    public bool? Unfriendly { get; set; }

    [JsonProperty("card_changeable")]
    public bool? CardChangeable { get; set; }

    [JsonProperty("is_robot")]
    public bool? IsRobot { get; set; }

    [JsonProperty("shut_up_timestamp")]
    public long? ShutUpTimestamp { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("message_id")]
    public long? MessageId { get; set; }

    [JsonProperty("birthday_year")]
    public int? BirthdayYear { get; set; }

    [JsonProperty("birthday_month")]
    public int? BirthdayMonth { get; set; }

    [JsonProperty("birthday_day")]
    [DataMember(Name = "birthday_day")]
    public int? BirthdayDay { get; set; }

    [JsonProperty("phone_num")]
    public string PhoneNum { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("category_id")]
    public int? CategoryId { get; set; }

    [JsonProperty("remark")]
    public string Remark { get; set; }

    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }
}
