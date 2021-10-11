using EventsApi.Interfaces;
using System;

namespace EventsApi.Validators
{
    public class DateValidator : IDateValidator
    {
        public bool ValidateFirstDateIsGreaterThanSecond(string firstDate, string secondDate)
        {
            DateTime parsedFirst;
            DateTime parsedSecond;

            if (DateTime.TryParse(firstDate, out parsedFirst))
            {
                return false;
            }
            if (DateTime.TryParse(secondDate, out parsedSecond))
            {
                return false;
            }
            return ValidateFirstDateIsGreaterThanSecond(parsedFirst, parsedSecond);
        }

        public bool ValidateFirstDateIsGreaterThanSecond(string firstDate, DateTime secondDate)
        {
            DateTime parsedFirst;

            if (DateTime.TryParse(firstDate, out parsedFirst))
            {
                return false;
            }
            return ValidateFirstDateIsGreaterThanSecond(parsedFirst, secondDate);
        }

        public bool ValidateFirstDateIsGreaterThanSecond(DateTime firstDate, DateTime secondDate)
        {
            int result = DateTime.Compare(firstDate, secondDate);
            return result >= 0;
        }
    }
}
