using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFriendServiceV2
    {
        int AddV2(FriendAddRequestV2 model, int currentUser);
        void DeleteV2(int newFriendId);
        List<FriendV2> GetAllV3();
        FriendV2 GetV3(int id);
        void UpdateV2(FriendUpdateRequestV2 model, int currentUser);
        public Paged<FriendV2> GetAllV3Pagination(int pageIndex, int pageSize);

        public Paged<FriendV2> SearchPaginationV3(int pageIndex, int pageSize, string query);
    }
}