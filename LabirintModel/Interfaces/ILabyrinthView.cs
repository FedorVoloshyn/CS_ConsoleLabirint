using System;
using LabyrinthModel.Enums;

namespace LabyrinthModel.Interfaces
{
    public interface ILabyrinthView
    {
        void PrintMessage(string message);
        void PrintLabyrinth(Labyrinth args);
        void ClearCanvas();
        KeyboardGameControls GetDirection();

        event EventHandler<ButtonEventArgs> ButtonPressed;
    }
}
