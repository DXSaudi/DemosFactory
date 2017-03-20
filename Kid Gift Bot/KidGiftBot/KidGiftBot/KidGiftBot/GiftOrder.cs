using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
#pragma warning disable 649


namespace KidGiftBot
{
    public enum GiftOptions
    {
        [Describe(Image = @"http://n.nordstrommedia.com/imagegallery/store/product/Zoom/7/_100179307.jpg")]
        Bike,
        [Describe(Image = @"http://nord.imgix.net/Zoom/17/_100368897.jpg")]
        CastleToy,
        [Describe(Image = @"http://content.nordstrom.com/imagegallery/store/product/large/14/_12807854.jpg")]
        Bunny,
        [Describe(Image = @"https://s-media-cache-ak0.pinimg.com/736x/c9/f2/7f/c9f27f2418e74da8d4505ccc6664a143.jpg")]
        Train,
        [Describe(Image = @"http://n.nordstrommedia.com/imagegallery/store/product/Large/14/_100135174.jpg")]
        Tent,
        [Describe(Image = @"http://nord.imgix.net/Zoom/6/_11563466.jpg?fit=fill&fm=jpg&dpr=2&h=368&w=240&q=30")]
        Puzzles
    };

    [Serializable]
    public class GiftOrder
    {
        [Prompt("What kind of {&} would you like? {||}")]
        public GiftOptions? Gift;
        [Numeric(1, 5)]
        public int Quantity;
        public static IForm<GiftOrder> BuildForm()
        {
            return new FormBuilder<GiftOrder>()
                    .Message("Welcome to the kid gift order bot!")
                    .Build();
        }
    };
}