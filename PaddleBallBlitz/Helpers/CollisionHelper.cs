using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PaddleBallBlitz.Helpers
{
    public static class CollisionHelper
    {

        public static Intersection CircleIntersectsRect(Vector2 start, Vector2 end, float radius, Rectangle bounds)
        {
             float L = bounds.X - bounds.Width / 2;
             float T = bounds.Y - bounds.Height / 2;
             float R = bounds.X + bounds.Width / 2;
             float B = bounds.Y + bounds.Height / 2;

            // If the bounding box around the start and end points (+radius on all
            // sides) does not intersect with the rectangle, definitely not an
            // intersection
            if ((Math.Max(start.X, end.X) + radius < L) ||
                (Math.Min(start.X, end.X) - radius > R) ||
                (Math.Max(start.Y, end.Y) + radius < T) ||
                (Math.Min(start.Y, end.Y) - radius > B))
            {
                return null;
            }

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float invdx = (dx == 0.0f ? 0.0f : 1.0f / dx);
            float invdy = (dy == 0.0f ? 0.0f : 1.0f / dy);
            float cornerX = float.MaxValue;
            float cornerY = float.MaxValue;


            // Calculate intersection times with each side's plane
            // Check each side's quadrant for single-side intersection
            // Calculate Corner

            /** Left Side **/
            // Does the circle go from the left side to the right side of the rectangle's left?
            if (start.X - radius < L && end.X + radius > L)
            {
                float ltime = ((L - radius) - start.X) * invdx;
                if (ltime >= 0.0f && ltime <= 1.0f)
                {
                    float ly = dy * ltime + start.Y;
                    // Does the collisions point lie on the left side?
                    if (ly >= T && ly <= B)
                    {
                        return new Intersection(dx * ltime + start.X, ly, ltime, -1, 0, L, ly);
                    }
                }
                cornerX = L;
            }

            /** Right Side **/
            // Does the circle go from the right side to the left side of the rectangle's right?
            if (start.X + radius > R && end.X - radius < R)
            {
                float rtime = (start.X - (R + radius)) * -invdx;
                if (rtime >= 0.0f && rtime <= 1.0f)
                {
                    float ry = dy * rtime + start.Y;
                    // Does the collisions point lie on the right side?
                    if (ry >= T && ry <= B)
                    {
                        return new Intersection(dx * rtime + start.X, ry, rtime, 1, 0, R, ry);
                    }
                }
                cornerX = R;
            }

            /** Top Side **/
            // Does the circle go from the top side to the bottom side of the rectangle's top?
            if (start.Y - radius < T && end.Y + radius > T)
            {
                float ttime = ((T - radius) - start.Y) * invdy;
                if (ttime >= 0.0f && ttime <= 1.0f)
                {
                    float tx = dx * ttime + start.X;
                    // Does the collisions point lie on the top side?
                    if (tx >= L && tx <= R)
                    {
                        return new Intersection(tx, dy * ttime + start.Y, ttime, 0, -1, tx, T);
                    }
                }
                cornerY = T;
            }

            /** Bottom Side **/
            // Does the circle go from the bottom side to the top side of the rectangle's bottom?
            if (start.Y + radius > B && end.Y - radius < B)
            {
                float btime = (start.Y - (B + radius)) * -invdy;
                if (btime >= 0.0f && btime <= 1.0f)
                {
                    float bx = dx * btime + start.X;
                    // Does the collisions point lie on the bottom side?
                    if (bx >= L && bx <= R)
                    {
                        return new Intersection(bx, dy * btime + start.Y, btime, 0, 1, bx, B);
                    }
                }
                cornerY = B;
            }

            // No intersection at all!
            if (cornerX == float.MaxValue && cornerY == float.MaxValue)
            {
                return null;
            }

            // Account for the times where we don't pass over a side but we do hit it's corner
            if (cornerX != float.MaxValue && cornerY == float.MaxValue)
            {
                cornerY = (dy > 0.0f ? B : T);
            }
            if (cornerY != float.MaxValue && cornerX == float.MaxValue)
            {
                cornerX = (dx > 0.0f ? R : L);
            }

            /* Solve the triangle between the start, corner, and intersection point.
       *                    
       *           +-----------T-----------+
       *           |                       |
       *          L|                       |R
       *           |                       |
       *           C-----------B-----------+
       *          / \      
       *         /   \r     _.-E
       *        /     \ _.-'
       *       /    _.-I
       *      / _.-'
       *     S-'
       *
       * S = start of circle's path
       * E = end of circle's path
       * LTRB = sides of the rectangle
       * I = {ix, iY} = point at which the circle intersects with the rectangle
       * C = corner of intersection (and collision point)
       * C=>I (r) = {nx, ny} = radius and intersection normal
       * S=>C = cornerdist
       * S=>I = intersectionDistance
       * S=>E = lineLength
       * <S = innerAngle
       * <I = angle1
       * <C = angle2
       */

            double inverseRadius = 1.0 / radius;
            double lineLength = Math.Sqrt(dx * dx + dy * dy);
            double cornerdx = cornerX - start.X;
            double cornerdy = cornerY - start.Y;
            double cornerDistance = Math.Sqrt(cornerdx * cornerdx + cornerdy * cornerdy);
            double innerAngle = Math.Acos((cornerdx * dx + cornerdy * dy) / (lineLength * cornerDistance));

            // If the circle is too close, no intersection.
            if (cornerDistance < radius)
            {
                return null;
            }

            float time;
            float ix;
            float iy;
            float nx;
            float ny;

            // If inner angle is zero, it's going to hit the corner straight on.
            if (innerAngle == 0.0f)
            {
                time = (float)((cornerDistance - radius) / lineLength);

                // If time is outside the boundaries, return null. This algorithm can
                // return a negative time which indicates a previous intersection, and
                // can also return a time > 1.0f which can predict a corner intersection.
                if (time > 1.0f || time < 0.0f)
                {
                    return null;
                }

                ix = time * dx + start.X;
                iy = time * dy + start.Y;
                nx = (float)(cornerdx / cornerDistance);
                ny = (float)(cornerdy / cornerDistance);

                return new Intersection(ix, iy, time, nx, ny, cornerX, cornerY);
            }

            double innerAngleSin = Math.Sin(innerAngle);
            double angle1Sin = innerAngleSin * cornerDistance * inverseRadius;

            // The angle is too large, there cannot be an intersection
            if (Math.Abs(angle1Sin) > 1.0f)
            {
                return null;
            }

            double angle1 = Math.PI - Math.Asin(angle1Sin);
            double angle2 = Math.PI - innerAngle - angle1;
            double intersectionDistance = radius * Math.Sin(angle2) / innerAngleSin;

            // Solve for time
            time = (float)(intersectionDistance / lineLength);

            // If time is outside the boundaries, return null. This algorithm can
            // return a negative time which indicates a previous intersection, and
            // can also return a time > 1.0f which can predict a corner intersection.
            if (time > 1.0f || time < 0.0f)
            {
                return null;
            }

            // Solve the intersection and normal
            ix = time * dx + start.X;
            iy = time * dy + start.Y;
            nx = (float)((ix - cornerX) * inverseRadius);
            ny = (float)((iy - cornerY) * inverseRadius);

            return new Intersection(ix, iy, time, nx, ny, cornerX, cornerY);
        }

        public static ColDir? Intersects(Circle circle, Rectangle rect)
        {















            //closes point on collision box
            float closestX, closestY;

            //find closest x offset
            if (circle.Center.X < rect.Left)
                closestX = rect.Left;
            else if (circle.Center.X > rect.Right)
                closestX = rect.Right;
            else
                closestX = circle.Center.X;

            //find closest y offset
            if (circle.Center.Y < rect.Top)
                closestY = rect.Top;
            else if (circle.Center.Y > rect.Bottom)
                closestY = rect.Bottom;
            else
                closestY = circle.Center.Y;
            
            //if the closest point is inside the circle
            if (DistanceSquared(circle.Center.X, circle.Center.Y, closestX, closestY) < circle.Radius*circle.Radius)
            {
                //collision
                if (closestX == rect.X && closestY > rect.Y && closestY < rect.Bottom)
                {
                    return ColDir.Left;
                }
                if (closestX > rect.X && closestX < rect.Right)
                {
                    if (closestY == rect.Top)
                        return ColDir.Top;
                    if (closestY == rect.Bottom)
                        return ColDir.Bottom;
                }
                if (closestX == rect.Right && closestY > rect.Y && closestY < rect.Bottom)
                {
                    return ColDir.Right;
                }
                if ((closestX == rect.X && closestY == rect.Y) || (closestX == rect.Right && closestY == rect.Y) || (closestX == rect.X && closestY == rect.Bottom) || (closestX == rect.Right && closestY == rect.Bottom))
                    return  ColDir.Corner;
            }

            return null;















            var NearestX = Math.Max(rect.X, Math.Min(circle.Center.X, rect.X + rect.Width));
            var NearestY = Math.Max(rect.Y, Math.Min(circle.Center.Y, rect.Y + rect.Height));

            var DeltaX = circle.Center.X - Math.Max(rect.X, Math.Min(circle.Center.X, rect.X + rect.Width));
            var DeltaY = circle.Center.Y - Math.Max(rect.Y, Math.Min(circle.Center.Y, rect.Y + rect.Height));

            bool intersects = (DeltaX * DeltaX + DeltaY * DeltaY) < (circle.Radius * circle.Radius);

            if (intersects)
            {
                //if (NearestX == rect.X && NearestY > rect.Y && NearestY < rect.Bottom)
                //{
                //    return ColDir.Left;
                //}
                //if (NearestX > rect.X && NearestX < rect.Right)
                //{
                //    if (NearestY == rect.Top)
                //        return ColDir.Top;
                //    if (NearestY == rect.Bottom)
                //        return ColDir.Bottom;
                //}
                //if (NearestX == rect.Right && NearestY > rect.Y && NearestY < rect.Bottom)
                //{
                //    return ColDir.Right;
                //}
            }

            //var circleDistanceX = Math.Abs(circle.Center.X - rect.X);
            //var circleDistanceY = Math.Abs(circle.Center.Y - rect.Y);

            //if (circleDistanceX > (rect.Width / 2 + circle.Radius)) { return ColDir.Right; }
            //if (circleDistanceY > (rect.Height / 2 + circle.Radius)) { return ColDir.Top; }

            //if (circleDistanceX <= (rect.X + (rect.Width / 2))) { return ColDir.Left; }
            //if (circleDistanceY <= (rect.Y + (rect.Height / 2))) { return ColDir.Bottom; }

            //var cornerDistanceSq = Math.Pow(circleDistanceX - (rect.X + (rect.Width / 2)), 2) +
            //                     Math.Pow(circleDistanceY - (rect.Y + (rect.Height / 2)), 2);

            //if (cornerDistanceSq <= Math.Pow(circle.Radius, 2))
            //    return ColDir.Corner;
            return null;
        }

        private static double DistanceSquared(float x1, float y1, float x2, float y2)
        {
            var deltaX = x2 - x1;
            var deltaY = y2 - y1;

            return deltaX * deltaX + deltaY * deltaY;
        }
    }
}
