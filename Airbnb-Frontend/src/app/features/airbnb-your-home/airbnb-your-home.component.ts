import { Component } from '@angular/core';
import { AirbnbYourHomeHeaderComponent } from "../airbnb-your-home-header/airbnb-your-home-header.component";
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-airbnb-your-home',
  imports: [AirbnbYourHomeHeaderComponent, RouterOutlet],
  templateUrl: './airbnb-your-home.component.html',
  styleUrl: './airbnb-your-home.component.css'
})
export class AirbnbYourHomeComponent {

}
