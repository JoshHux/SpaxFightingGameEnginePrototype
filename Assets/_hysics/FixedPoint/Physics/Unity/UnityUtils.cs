using System.Collections.Generic;
using System;
using System.Reflection;

namespace Spax {

    /**
    *  @brief Provides a few utilities to be used on FixedPoint exposed classes.
    **/
    public class UnityUtils {

        /**
         *  @brief Comparer class to guarantee {@link FPCollider} order.
         **/
        public class FPBodyComparer : Comparer<FPCollider> {

            public override int Compare(FPCollider x, FPCollider y) {
                return x.gameObject.name.CompareTo(y.gameObject.name);
            }

        }

        /**
         *  @brief Comparer class to guarantee {@link FPCollider2D} order.
         **/
        public class FPBody2DComparer : Comparer<FPCollider2D> {

            public override int Compare(FPCollider2D x, FPCollider2D y) {
                return x.gameObject.name.CompareTo(y.gameObject.name);
            }

        }

        /**
         *  @brief Instance of a {@link FPBodyComparer}.
         **/
        public static FPBodyComparer bodyComparer = new FPBodyComparer();

        /**
         *  @brief Instance of a {@link FPBody2DComparer}.
         **/
        public static FPBody2DComparer body2DComparer = new FPBody2DComparer();

    }

}