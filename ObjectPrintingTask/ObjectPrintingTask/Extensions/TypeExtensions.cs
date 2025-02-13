﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectPrintingTask.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<MemberInfo> GetPropertiesAndFields(this Type type)
        {
            return type.GetMembers().Where(member => member.IsFieldOrProperty()).ToList();
        }
    }
}