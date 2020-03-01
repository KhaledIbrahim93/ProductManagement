import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ProductResponse } from '../Model/product-response';
import { ProductRequest } from '../Model/product-request';

const API_PRODUCT_URL = environment.API_URL;
const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
@Injectable()

export class ProductService {

  constructor(private http: HttpClient) { }

  getAllProducts(): Observable<any> {

    var response = this.http.get<ProductResponse[]>(API_PRODUCT_URL + '/Get');
    return response;
  }
  createProduct(product: ProductRequest, Image: any): Observable<any> {
    const formData = new FormData();
    formData.append('file', Image);
    formData.append('info', JSON.stringify(product));
    return this.http.post<any>(API_PRODUCT_URL + '/AddProduct', formData)
  }
  deleteProduct(id: any): Observable<any> {
    return this.http.delete<any>(API_PRODUCT_URL + '/Delete/' + id, httpOptions);
  }
  getProductById(id: any): Observable<any> {
    return this.http.get<any>(API_PRODUCT_URL + '/GetById/' + id, httpOptions);
  }
  editProduct(prdocut:any,image:any):Observable<any>
  {
    const editdata=new FormData()
    editdata.append('file', image);
    editdata.append('info', JSON.stringify(prdocut));
   return this.http.put<any>(API_PRODUCT_URL + '/Put',editdata)
  } 
  search(value:string):Observable<any>
  {
    return this.http.get<any>(API_PRODUCT_URL + '/Search/' + value, httpOptions)
  }
}
