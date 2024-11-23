public static class LoginInformation
{
    private static User _loggedUser;

    public static User LoggedUser
    {
        get
        {
            return _loggedUser;
        }
        set
        {
            isLoggedAsGuest = value == null;
            _loggedUser = value;
        }
    }
    public static bool isLoggedAsGuest { get; set; }
}
