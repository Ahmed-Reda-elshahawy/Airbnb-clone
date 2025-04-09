import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./shared/components/header/header.component";
import { ListingCardComponent } from "./features/listing-card/listing-card.component";
import { FooterComponent } from "./shared/components/footer/footer.component";
import { HomeComponent } from './features/home/home.component';



@Component({
  selector: 'app-root',
  standalone:true,
  imports: [RouterOutlet,FooterComponent, ListingCardComponent, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Airbnb-Frontend';
}
