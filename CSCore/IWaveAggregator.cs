﻿namespace CSCore
 {
     /// <summary>
     ///     Defines the base interface for any <see cref="IWaveSource" /> aggregators.
     /// </summary>
     public interface IWaveAggregator : IWaveSource
     {
         /// <summary>
         ///     Gets the underlying <see cref="IWaveSource" />.
         /// </summary>
         IWaveSource BaseStream { get; }
     }
 }