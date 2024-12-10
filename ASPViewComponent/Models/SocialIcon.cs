namespace ASPViewComponent.Models;

public class SocialIcon
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string BackgroundColor { get; set; }
    public string TargetUrl { get; set; }
    public string Class { get; set; }
 
    public static List<SocialIcon> AppSocialIcons()
    {
        return new List<SocialIcon>()
        {
            new SocialIcon { ID = 1, Name = "Google", BackgroundColor = "#dd4b39", TargetUrl = "www.google.com", Class = "fa fa-google" },
            new SocialIcon { ID = 2, Name = "Facebook", BackgroundColor = "#3B5998", TargetUrl = "www.facebook.com", Class = "fa fa-facebook" },
            new SocialIcon { ID = 3, Name = "Linked In", BackgroundColor = "#007bb5", TargetUrl = "www.linkedin.com", Class = "fa fa-fa-linkedin" },
            new SocialIcon { ID = 4, Name = "YouTube", BackgroundColor = "#007bb5", TargetUrl = "www.youtube.com", Class = "fa fa-youtube" },
            new SocialIcon { ID = 5, Name = "Twitter", BackgroundColor = "#55acee", TargetUrl = "www.twitter.com", Class = "fa fa-twitter" }
        };
    }
}