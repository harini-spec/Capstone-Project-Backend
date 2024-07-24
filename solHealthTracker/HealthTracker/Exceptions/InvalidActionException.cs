﻿using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class InvalidActionException : Exception
    {
        public InvalidActionException()
        {
        }

        public InvalidActionException(string? message) : base(message)
        {
        }
    }
}