﻿using Model.VM;

namespace Model.Out
{
    public class UserEditOut : ResultServiceVM
    {
        public UserEditOut()
        {
            result = new UserEditVM();
        }

        public UserEditVM result { get; set; }
    }
}
