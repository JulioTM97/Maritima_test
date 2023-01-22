# Maritima_test
Prueba técnica para Marítima Dominicana.
Este proyecto se divide en dos partes:

- **AngularApp:** Contiene la aplicacion desarrollada en Angular para consultar la lista de empleados activos en el sistema.
- **RestApi:** Es el proyecto ASP.Net Core Web Api de .Net 6 donde se encuentra la Web Api construida en Visual Studio.

Para cambiar la ruta desde donde la aplicación de Angular hace la consulta a la Api solo debe modificar el archivo **AngularApp/src/app/components/MasterCompanyEmployees/MasterCompanyEmployees.component.ts** el cual contiene una propiedad llamada **urlToRequest,** solo debe reemplazar esa cadena con la nueva.
