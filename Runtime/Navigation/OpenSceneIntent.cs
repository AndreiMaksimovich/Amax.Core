// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

namespace Amax.Navigation
{
    public class OpenSceneIntent
    {
        
        public Scene Scene { get; set; }
        public bool ClearHistory { get; set; }
        public bool AddCurrentSceneToHistory { get; set; } = true;
        public bool ActivateSceneImmediately { get; set; } = true;

    }
}