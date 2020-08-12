using System;

using Audit.Common.Interfaces;

namespace Audit.API.Infrastructure
{
    public class IMockUserAccessor : IUserAccessor
    {
        private static Random r = new Random();

        public int GetCurrentUserId => r.Next(1000);
    }
}
