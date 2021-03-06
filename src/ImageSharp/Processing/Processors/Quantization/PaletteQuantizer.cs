// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Processing.Processors.Quantization
{
    /// <summary>
    /// Allows the quantization of images pixels using color palettes.
    /// </summary>
    public class PaletteQuantizer : IQuantizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteQuantizer"/> class.
        /// </summary>
        /// <param name="palette">The color palette.</param>
        public PaletteQuantizer(ReadOnlyMemory<Color> palette)
            : this(palette, new QuantizerOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteQuantizer"/> class.
        /// </summary>
        /// <param name="palette">The color palette.</param>
        /// <param name="options">The quantizer options defining quantization rules.</param>
        public PaletteQuantizer(ReadOnlyMemory<Color> palette, QuantizerOptions options)
        {
            Guard.MustBeGreaterThan(palette.Length, 0, nameof(palette));
            Guard.NotNull(options, nameof(options));

            this.Palette = palette;
            this.Options = options;
        }

        /// <summary>
        /// Gets the color palette.
        /// </summary>
        public ReadOnlyMemory<Color> Palette { get; }

        /// <inheritdoc />
        public QuantizerOptions Options { get; }

        /// <inheritdoc />
        public IFrameQuantizer<TPixel> CreateFrameQuantizer<TPixel>(Configuration configuration)
            where TPixel : struct, IPixel<TPixel>
            => this.CreateFrameQuantizer<TPixel>(configuration, this.Options);

        /// <inheritdoc />
        public IFrameQuantizer<TPixel> CreateFrameQuantizer<TPixel>(Configuration configuration, QuantizerOptions options)
            where TPixel : struct, IPixel<TPixel>
        {
            Guard.NotNull(options, nameof(options));

            int length = Math.Min(this.Palette.Span.Length, options.MaxColors);
            var palette = new TPixel[length];

            Color.ToPixel(configuration, this.Palette.Span, palette.AsSpan());
            return new PaletteFrameQuantizer<TPixel>(configuration, options, palette);
        }
    }
}
