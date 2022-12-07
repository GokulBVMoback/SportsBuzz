using Entities.Models;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface IChallengeTeam
    {
        List<TeamList> SearchAvailableTeam(SearchAvailableGround availablTeam);
        bool ChallengingTeam(TblChallenge challenge);
        Notification GenerateMessage(TblChallenge challenge);
        void AcceptTeamRequest(int challengeId);
        void RejectTeamRequest(int challengeId);
        bool CheckExtistChallengedDetails(TblChallenge challenge);
    }
}
