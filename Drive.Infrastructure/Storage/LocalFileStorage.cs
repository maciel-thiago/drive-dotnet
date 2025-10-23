namespace Drive.Infrastructure.Storage;

public class LocalFileStorage { }


/**
 *
 *
 *
 *
 interface Request {
  pathParameters: {}

  queryStringParameters: {}

  body: {
      title: string,
      size: number,
      file: {
        type: string,
        base64: string // Remove metadata information
  }
}

interface Response {
    statusCode: 204
}

interface Request {
  pathParameters: {}

  queryStringParameters: {
      pageSize: number
      pageNumber: number
    }

  body: {}
}

interface Response {
  files: GetFileResponse[]
  pageSize: number
  pageNumber: number
  totalPages: number
  totalCount: number
}
___________________________
interface Request {
  pathParameters: {}

  queryStringParameters: {
      pageSize: number
      pageNumber: number
    }

  body: {}
}

interface Response {
  files: GetFileResponse[]
  pageSize: number
  pageNumber: number
  totalPages: number
  totalCount: number
}
interface GetFileResponse {
  id: string // UUID
  name: string
  size: number
  fileType: string
  preSignedUrl: string
  createdAt: string // ISO Date
}

------------------------------------

interface Request {
  pathParameters: {
      id: string // UUID
    }

  queryStringParameters: {}

  body: {}
}


interface Response {
    statusCode: 204
}

 */
