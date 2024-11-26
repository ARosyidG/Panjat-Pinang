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
            _loggedUser = value;
            isLoggedAsGuest = _loggedUser == null;
        }
    }
    public static bool isLoggedAsGuest { get; set; }
    static LoginInformation(){
        LoggedUser = null;
    }
}
