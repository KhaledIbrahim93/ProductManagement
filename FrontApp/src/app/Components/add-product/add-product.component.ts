import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ProductRequest } from 'src/app/Model/product-request';
import { ProductService } from 'src/app/Service/product.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
  Image: any
  productform: any
  productRequest: any;
  ImagePath: any
  Extentions: string[] = ['jpg', 'png', 'gif'];
  fileExtenstion:string="";
  imageStatus:boolean=true;
  constructor(private formbulider: FormBuilder,private service:ProductService ,private router:Router) { }
  ngOnInit() {
    this.createform();
  }
  createform(){
    this.productform = this.formbulider.group({
      productName: ['', [Validators.required]],
      productPrice: ['', [Validators.required]],
      productImage: ['', [Validators.required]],
    });
  }
  onFileChanged(event) {
    debugger;
    this.Image = event.target.files[0];
    this.fileExtenstion = event.target.files[0].name.split('.')[1];
    if (!this.Extentions.includes(this.fileExtenstion)) 
    {
      this.imageStatus=false;
     alert("you Should Upload Image");
    }
  }
  preperProduct() {
    debugger;
    const controls = this.productform.controls;
    const _product = new ProductRequest();
    _product.Name = controls['productName'].value;
    _product.Price = controls['productPrice'].value;
    return _product;
  }
  createProduct(){
    debugger;
    if(this.imageStatus)
    {
      this.productRequest=this.preperProduct();
      this.service.createProduct(this.productRequest,this.Image).subscribe(res=>{
          this.router.navigate(['/GetProducts']);
      });
    }
    else{
      alert("you must Upload Image");
      this.imageStatus=true;
    }
  }
}


