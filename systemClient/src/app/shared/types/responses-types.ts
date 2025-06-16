import { Product } from './products-types';

export interface responseMessage {
  message: string;
}

export interface responseData<T = any> {
  message: string;
  data: T;
}
