using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture.Entities;
using Architecture.Entities.System;
using Architecture.Managers.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Architecture.Managers
{
    public class CubeManager : IManager
    {
        public ReadOnlyCollection<Cube> Elements => Cubes.AsReadOnly();
        protected readonly List<Cube> Cubes = new();
        protected readonly List<Cube> CubesToAdd = new();
        protected readonly List<Cube> CubesToRemove = new();

        protected readonly List<Cube> IgnoringCubes = new();
        protected readonly List<Cube> CubesToIgnore = new();
        protected readonly List<Cube> CubesToDisableIgnore = new();

        private readonly Camera _camera;
        internal Camera Camera => _camera;
        private bool _press;

        public void Add(Cube cube) =>
            CubesToAdd.Add(
                cube ?? throw new ArgumentNullException(nameof(cube), "Null cube cannot be added"));

        public void Remove(Cube cube) =>
            CubesToAdd.Add(
                cube ?? throw new ArgumentNullException(nameof(cube), "Null cube cannot be removed"));

        public void Ignore(Cube cube) =>
            CubesToIgnore.Add(
                cube ?? throw new ArgumentNullException(nameof(cube), "Null cube cannot be ignoring"));

        public void DisableIgnore(Cube cube) =>
            CubesToDisableIgnore.Add(
                cube ?? throw new ArgumentNullException(nameof(cube), "Null cube cannot be ignoring"));

        public IEnumerable<Cube> Ignoring() => IgnoringCubes;

        public CubeManager(GraphicsDevice graphics, IEnumerable<Cube> cubes, float distancing)
        {
            _camera = new Camera(graphics.Viewport.AspectRatio,
                distancing, MathHelper.ToRadians(60), new Vector3(0, 5, 0));
            foreach (var el in cubes)
                Add(el);
        }

        public void OnButtonPress() => _press = true;
        public void OnButtonRelease() => _press = false;

        public void OnLeftArrowPress() => RotateCamera(MathHelper.ToRadians(-1));
        public void OnRightArrowPress() => RotateCamera(MathHelper.ToRadians(1));

        

        public void Manage(GameTime gameTime, Screen screen)
        {
            foreach (var cube in Cubes)
            {
                cube.IsHovered = !IgnoringCubes.Contains(cube) && 
                                 cube.CheckIntersection(screen, _camera) &&
                                 (cube.IsHovered || Cubes.All(c => !c.IsHovered)) ;
                cube.IsPressed = cube.IsHovered && _press;
                cube.Update(screen, _camera, gameTime);
            }
            
            foreach (var cube in CubesToAdd)
                Cubes.Add(cube);

            foreach (var cube in CubesToRemove)
                Cubes.Remove(cube);


            foreach (var cube in CubesToIgnore)
                IgnoringCubes.Add(cube);

            foreach (var cube in CubesToDisableIgnore)
                IgnoringCubes.Remove(cube);

            CubesToIgnore.Clear();
            CubesToAdd.Clear();
            CubesToRemove.Clear();
            CubesToDisableIgnore.Clear();
        }

        public void RotateCamera(float angleInRadians) => _camera.Rotate(angleInRadians);

        public void Clear() => CubesToRemove.AddRange(Cubes);

        public void DrawCubes()
        {
            var sortedCubes = Cubes
                .OrderBy(cube => -Vector3.DistanceSquared(cube.World.Translation, _camera.Position));
            foreach (var cube in sortedCubes)
                cube.Draw(_camera);
        }
    }
}
