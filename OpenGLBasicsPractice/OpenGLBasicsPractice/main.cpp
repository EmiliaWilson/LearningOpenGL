#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "shader.h"

#include <iostream>

const unsigned int SCR_WIDTH = 1024;
const unsigned int SCR_HEIGHT = 768;

void process_input(GLFWwindow* window);

void framebuffer_size_callback(GLFWwindow* window, int width, int height);

int main() {
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearningOpenGL", NULL, NULL);
	if (window == NULL) {
		std::cout << "creating window falled" << std::endl;
		glfwTerminate();
		return -1;
	}

	glfwMakeContextCurrent(window);
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);


	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
		std::cout << "Failed to init GLAD" << std::endl;
		return -1;
	}

	float vertices[] = 
	{ 
		-0.5f, -0.5f, 0.0f,
		 0.5f, -0.5f, 0.0f,
		 0.0f,  0.5f, 0.0f
	};

	unsigned int VBO, VAO;
	glGenBuffers(1, &VBO);
	glGenVertexArrays(1, &VAO);

	glBindVertexArray(VAO);

	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);

	glEnableVertexAttribArray(0);


	Shader OurShader = Shader("vertexShader.vs", "fragmentShader.fs");

	while (!glfwWindowShouldClose(window)) {

		process_input(window);
		glClearColor(0.4f, 0.1f, 0.4f, 0.0f);
		glClear(GL_COLOR_BUFFER_BIT);

		OurShader.use();

		glBindVertexArray(VAO);
		glDrawArrays(GL_TRIANGLES, 0, 3);

		
		glfwPollEvents();
		glfwSwapBuffers(window);
	}
	glfwTerminate();
	return 0;
}

void process_input(GLFWwindow* window) {
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS) {
		glfwSetWindowShouldClose(window, true);
	}
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height) {
	glViewport(0, 0, width, height);
}