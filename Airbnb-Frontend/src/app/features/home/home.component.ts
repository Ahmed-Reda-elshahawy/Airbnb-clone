import { Component } from '@angular/core';
import { ListingCardComponent } from '../listing-card/listing-card.component';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-home',
  standalone:true,
  imports: [ListingCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  // constructor(public apiService: ApiService) {}

  // ngOnInit(){
  //   this.apiService.loadListings();
  // }
}
