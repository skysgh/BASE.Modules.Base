using App.Modules.Base.Substrate.Contracts.Models;

namespace App.Modules.Base.Substrate.Models.Entities.Base
{
    /// <summary>
    /// TODO: Improve documentation
    /// </summary>
    public abstract class SettingBase :
        UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasKey,
        IHasEnabled,
        IHasSerializedTypeValueNullable
    {
        /// <inheritdoc/>
        public bool Enabled { get; set; }
        /// <inheritdoc/>
        public string Key { get; set; }
        /// <inheritdoc/>

        public string SerializedTypeName { get; set; }
        /// <inheritdoc/>
        public string? SerializedTypeValue { get; set; }


        /// <summary>
        /// TODO: Improve documentation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SetValue<T>(T value)
        {
            SerializedTypeName = typeof(T).ToString();
            _value = value;
        }
        private object? _value;

        /// <summary>
        /// TODO: Improve documentation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            // Has been set, so SerializedTypeName is correct,
            // can get out early:
            if (_value != null)
            {
                return (T)_value;
            }
            // otherwise deserialize:
            //_value = <default>T;

            SerializedTypeName = typeof(T).ToString();

            // Then get out as best as one can:
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
            return (T)_value;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
    }
}