import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {ProductService} from '../../Service/product.service'
import { from } from 'rxjs';
import { ProductResponse } from 'src/app/Model/product-response';
import { DebugRenderer2 } from '@angular/core/src/view/services';
import { Router } from '@angular/router';
import { FormBuilder ,Validators} from '@angular/forms';
import { environment } from 'src/environments/environment';
import * as XLSX from 'xlsx';  

const url=environment.HostURL;
@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})

export class ProductDetailsComponent implements OnInit {
  allProducts: ProductResponse[]=[];
  url:string="";
  @ViewChild('TABLE') TABLE: ElementRef;  
  title = 'Excel';  
  constructor(private service:ProductService,private router:Router) { 
    // this.AllProducts=new ProductResponse();
  }
  ngOnInit() {
  
   this.GetProducts();
   this.url=url
  }
  ExportTOExcel() {  
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.TABLE.nativeElement);  
    const wb: XLSX.WorkBook = XLSX.utils.book_new();  
    XLSX.utils.book_append_sheet(wb, ws, 'ProductSheet');  
    XLSX.writeFile(wb, 'ProductSheet.xlsx');  
  }  
  GetProducts()
  {
    this.service.getAllProducts().subscribe(data=>{
    this.allProducts=data;
   })
  }
  deleteProduct(id)
  {
    this.service.deleteProduct(id).subscribe(res=>{
     this.allProducts.splice(this.allProducts.indexOf(id));
    })
  }
  editProduct(id){

    this.router.navigate(['/editProduct/'+id]);
  }
  search(value)
  {
    this.service.search(value).subscribe(data=>{
      if(!data){
        this.GetProducts();
      }
      this.allProducts=data;
    })
  }
  checkValue(){
    alert("dsgdsgdfdfdf");
  }
}
