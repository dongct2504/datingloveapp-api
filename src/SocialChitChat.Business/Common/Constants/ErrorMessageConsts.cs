namespace SocialChitChat.Business.Common.Constants;

public static class ErrorMessageConsts
{
    public const string UserNotFound = "Không tìm thấy người dùng.";
    public const string AdminNotFound = "Không tìm thấy chủ nhóm.";
    public const string SenderNotFound = "Không tìm thấy người gửi.";
    public const string ChatYourselfError = "Bạn không thể tự chat bản thân mình được.";
    public const string RecipientNotFound = "Không tìm thấy người nhận.";
    public const string UserDuplicate = "Người dùng đã tồn tại.";

    public const string InvalidPredicate = "Predicate không hợp lệ.";

    public const string LikeYourselfError = "Bạn không thể tự like mình được.";
    public const string SourceUserNotFound = "Không tìm thấy người đã like.";
    public const string LikedUserNotFound = "Không tìm thấy người đã like mình.";

    public const string PictureNotFound = "Không tìm thấy ảnh.";
    public const string AlreadyMainPicture = "Bức ảnh này đã là ảnh chính.";
    public const string NotAllowDeleteMainPicture = "Bạn không thể xóa ảnh chính của bạn.";

    public const string ConversationNotFound = "Không tìm thấy cuộc trò truyện.";
    public const string MessageNotFound = "Không tìm thấy tin nhắn.";

    public const string WrongUserName = "Sai tên đăng nhập.";
    public const string WrongPassword = "Sai mật khẩu.";

    public const string GroupChatNotFound = "Không tìm thấy nhóm.";
    public const string GroupChatCannotRemove = "Xóa nhóm không thành công.";

    public const string ParticipantChatNotFound = "Không tìm thấy thành viên trong nhóm.";

    public const string PostNotFound = "Không tìm thấy bài đăng.";
    public const string TooManyPostPictures = "Vui lòng tải ít hơn hoặc bằng 10 hình ảnh cho 1 bài đăng.";
}
