import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ProductService } from 'src/app/Service/product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductRequest } from 'src/app/Model/product-request';
import { environment } from 'src/environments/environment';
const url=environment.HostURL;
@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {
  productform: any;
  product: any;
  productId: number;
  url: string = ""
  Image: any
  Extentions: string[] = ['jpg', 'png', 'gif'];
  fileExtenstion: string = "";
  imageStatus: boolean = true;
  constructor(
    private formbulider: FormBuilder, 
    private service: ProductService, 
    private route: ActivatedRoute,
    private router:Router) {
    this.productId = parseInt(this.route.snapshot.paramMap.get('id'));
  }

  ngOnInit() {
    this.createform();
    this.getProductById();
    this.url=url;
  }
  createform() {
    this.productform = this.formbulider.group({
      productName: ['', [Validators.required]],
      productPrice: ['', [Validators.required]],
      productImage: ['', [Validators.required]],
    });
  }
  onFileChanged(event) 
  {
   debugger;
    this.Image = event.target.files[0];
    this.fileExtenstion = event.target.files[0].name.split('.')[1];
    if (!this.Extentions.includes(this.fileExtenstion)) {
      this.imageStatus = false;
      alert("you Should Upload Image");
    }
  }
  getProductById() {
    debugger;
    this.service.getProductById(this.productId).subscribe(data => {
      console.log(data);
      this.productform.controls['productName'].setValue(data.name);
      this.productform.controls['productPrice'].setValue(data.price);
      this.url = this.url+data.url
    })
  }
  preperProduct() {
    debugger;
    const controls = this.productform.controls;
    const _product = new ProductRequest();
    _product.Name = controls['productName'].value;
    _product.Price = controls['productPrice'].value;
    _product.Id=this.productId;
    return _product;
  }
  editProduct(){
    debugger;
    var product=this.preperProduct();
    this.service.editProduct(product,this.Image).subscribe(data=>{
      this.router.navigate(['/GetProducts']);
    })
  }

}
