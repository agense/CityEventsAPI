using System;

namespace EventsApi.Interfaces
{
    public interface IDateValidator
    {
        public bool ValidateFirstDateIsGreaterThanSecond(string firstDate, string SecondDate);

        public bool ValidateFirstDateIsGreaterThanSecond(string firstDate, DateTime SecondDate);

        public bool ValidateFirstDateIsGreaterThanSecond(DateTime firstDate, DateTime SecondDate);
    }
}
