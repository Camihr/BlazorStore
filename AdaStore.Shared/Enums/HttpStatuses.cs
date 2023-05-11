using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaStore.Shared.Enums
{
    public enum HttpStatuses
    {
        Ok = 200,// OK: La solicitud se ha completado correctamente y se ha devuelto una respuesta.
        Created = 201,// Created: La solicitud se ha completado correctamente y se ha creado un nuevo recurso.
        NoContent = 204,// No Content: La solicitud se ha completado correctamente, pero no hay contenido que devolver.
        BadRequest = 400,// Bad Request: La solicitud es incorrecta o no se puede procesar por alguna razón.
        Unauthorized = 401,// Unauthorized: La solicitud requiere autenticación, pero no se han proporcionado credenciales válidas.
        Forbidden = 403,// Forbidden: El servidor ha entendido la solicitud, pero se niega a cumplirla debido a que el cliente no tiene permiso para acceder al recurso solicitado.
        NotFound = 404,// Not Found: El servidor no pudo encontrar el recurso solicitado.
        InternalServer = 500,// Internal Server Error: El servidor ha encontrado una situación inesperada que le impide cumplir con la solicitud.
        ServiceUnavailable = 503,// Service Unavailable: El servidor no puede procesar la solicitud en este momento debido a una sobrecarga temporal o mantenimiento del servidor.
    }
}
