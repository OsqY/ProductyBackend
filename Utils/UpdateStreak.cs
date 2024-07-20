using Producty.Models;

public class Utils
{
    private void UpdateStreak(AppUser user, DateTime sessionDate)
    {
        if (user.LastStudyDate.Date == sessionDate.Date.AddDays(-1))
            user.CurrentStreak++;
        else if (user.LastStudyDate.Date != sessionDate.Date)
            user.CurrentStreak = 1;

        if (user.CurrentStreak > user.LastStreak)
            user.LastStreak = user.CurrentStreak;

        user.LastStudyDate = sessionDate.Date;
    }
}
