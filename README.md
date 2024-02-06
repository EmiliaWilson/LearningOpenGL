# Sum of Sines Water Simulation in OpenGL Render
by Emilia Wilson

A C++ OpenGL real time rendering software running a sum of sines water simulation


https://github.com/EmiliaWilson/LearningOpenGL/assets/31546309/352b2e2f-2506-4e3c-8b60-2b8f95e91de2


# Rendering Engine 

This is a real time rendering engine utilizing OpenGL. It's implemention comes from [learnopenGL.com](learnopenGL.com). It includes basic scene and model loading, implements the blinn-phong lighting model, anti-aliasing, and supports post-processing effects. Supports directional light, point light sources, and flashlights.

# Sum of Sines Water Simulation

Water simulation implementation described in the first volume of [GPU Gems](https://developer.nvidia.com/gpugems/gpugems/part-i-natural-effects/chapter-1-effective-water-simulation-physical-models). 
It also utilizes [fractional brownian motion](https://thebookofshaders.com/13/) to help increase the fidelity of the simulation. The water also includes basic reflections from the skybox. In the post processing stage we are applying Bloom HDR tone mapping, and gamma correction. 

# Improvements
* Replace lighting model with a more modern solution (like ray marching).
* Utilize Level of detail to increase the tesselation of the mesh close to get finer detail, while decreasing far away parts of the mesh for performance improvements. 
* Implement fog to obscure the horizon. 
