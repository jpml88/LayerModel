# LayerModel

Patron de Estructura: Capas
1 Capa DataLayer: Acceso a base de datos y el Crud correspondiente.
2 Capa Negocio: Logica de negocio, donde muestra la logica que realiza una operación.

En el proyecto contamos con un WebService a modo ejemplo con dos metodos, el mismo llama a la logica de negocio y el mismo mediante una interfaz a la capa de datos. La repuesta que tiene el mismo esta compuesta por una respuesta Base ( Clase abstracta ) donde la misma sera heredada por la respuesta propio de ese meotodo con sus correspondientes objetos.

Juan Pablo Markiewicz
