// personal-info.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PersonalInfoService } from '../../core/services/personal-info.service';
import { User } from '../../core/models/user';

interface PersonalInfoSection {
  title: string;
  fields: PersonalInfoField[];
  editMode?: boolean ;
}

interface PersonalInfoField {
  label: string;
  value: string;
  type: 'text' | 'date' | 'select' | 'email' | 'tel' | 'textarea' | 'pass';
  placeholder?: string;
  required?: any;
  options?: {value: string, label: string}[];
  privacyInfo?: string;
}

@Component({
  selector: 'app-personal-info',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.css']
})
export class PersonalInfoComponent implements OnInit {

userData2:User={};

  constructor(private _PersonalInfoService:PersonalInfoService){}
 
  ngOnInit(): void {
    this.getMyPersonalInfo();
  }

  getMyPersonalInfo(){
    this._PersonalInfoService.getMyPersonalInfo().subscribe({
      next:(res)=>{
        this.userData2=res;
      },
      error:(err)=>{
        console.log(err)
    }})
  }

  userData = {
    name: 'John Hany',
    email: 'hanyjohn2001@gmail.com',
    password:'aa',
    updatePassword:'ss',
    confirmPassword:'ss',
    phone: '+1 (555) 123-4567',
    address: '123 Main St, New York, NY 10001',
    dob: '1990-01-15',
    gender: 'Male',
    language: 'English',
    currency: 'USD - United States Dollar',
    govId: 'Not provided'
  };



  
  personalInfoSections: PersonalInfoSection[] = [
    {
      title: 'Legal name',
      fields: [
        {
          label: 'Legal name',
          value: this.userData.name,
          type: 'text',
          required: true
        }
      ]
    },
    {
      title: 'Email address',

      fields: [
        {
          label: 'Email',
          value: this.userData.email,
          type: 'email',
          required: true,
          privacyInfo: 'This is visible only to you and Airbnb.'
        }
      ]
    },
        {
      title: 'Password',

      fields: [
        {
          label: ' password',
          value: this.userData.password,
          type: 'pass'
        },
        {
          label: 'update password',
          value: this.userData.updatePassword,
          type: 'pass'
        },  
        {
          label: 'confirm password',
          value: this.userData.confirmPassword,
          type: 'pass'
        }
      ],
      // editMode: true

    },
    {
      title: 'Phone numbers',

      fields: [
        {
          label: 'Phone',
          value: this.userData.phone,
          type: 'tel',
          privacyInfo: 'For notifications, reminders, and help logging in.'
        }
      ]
    },
    {
      title: 'Address',

      fields: [
        {
          label: 'Address',
          value: this.userData.address,
          type: 'textarea',
          privacyInfo: 'This is visible only to you and Airbnb.'
        }
      ]
    },
    {
      title: 'Personal info',

      fields: [
        {
          label: 'Date of birth',
          value: this.userData.dob,
          type: 'date',
          privacyInfo: 'Your birthdate is not shared with other Airbnb users.'
        },
        {
          label: 'Gender',
          value: this.userData.gender,
          type: 'select',
          options: [
            { value: 'Male', label: 'Male' },
            { value: 'Female', label: 'Female' },
            { value: 'PreferNotToSay', label: 'Prefer not to say' }
          ],
          privacyInfo: 'We use this data for analytics and never share it with other users.'
        }
      ]
    },
    {
      title: 'Language and currency',

      fields: [
        {
          label: 'Language',
          value: this.userData.language,
          type: 'select',
          options: [
            { value: 'English', label: 'English' },
            { value: 'Spanish', label: 'Spanish' },
            { value: 'French', label: 'French' },
            { value: 'German', label: 'German' }
          ]
        },
        {
          label: 'Currency',
          value: this.userData.currency,
          type: 'select',
          options: [
            { value: 'USD - United States Dollar', label: 'USD - United States Dollar' },
            { value: 'EUR - Euro', label: 'EUR - Euro' },
            { value: 'GBP - British Pound', label: 'GBP - British Pound' },
            { value: 'JPY - Japanese Yen', label: 'JPY - Japanese Yen' }
          ]
        }
      ]
    }
  ];

  toggleEditMode(section: PersonalInfoSection): void {
    section.editMode = !section.editMode;
  }

  saveSection(section: PersonalInfoSection): void {
    // Here you would typically save the data to your backend
    console.log('Saving section:', section);
    
    // Update the user data based on the fields
    section.fields.forEach(field => {
      // In a real app, you'd map each field to the correct userData property
      if (field.label === 'Legal name') this.userData.name = field.value;
      if (field.label === 'Email') this.userData.email = field.value;
      if (field.label === 'Phone') this.userData.phone = field.value;
      if (field.label === 'Address') this.userData.address = field.value;
      if (field.label === 'password') field.value = this.userData.password;
      if (field.label === 'update password') field.value = this.userData.updatePassword;
      if (field.label === 'confirm password') field.value = this.userData.confirmPassword;
      if (field.label === 'Date of birth') this.userData.dob = field.value;
      if (field.label === 'Gender') this.userData.gender = field.value;
      if (field.label === 'Language') this.userData.language = field.value;
      if (field.label === 'Currency') this.userData.currency = field.value;
    });

    // Exit edit mode
    section.editMode = false;
  }

  cancelEdit(section: PersonalInfoSection): void {
    // Reset field values to the original values
    section.fields.forEach(field => {
      if (field.label === 'Legal name') field.value = this.userData.name;
      if (field.label === 'Email') field.value = this.userData.email;
      if (field.label === 'Phone') field.value = this.userData.phone;
      if (field.label === 'Address') field.value = this.userData.address;
      if (field.label === 'password') field.value = this.userData.password;
      if (field.label === 'update password') field.value = this.userData.updatePassword;
      if (field.label === 'confirm password') field.value = this.userData.confirmPassword;
      if (field.label === 'Date of birth') field.value = this.userData.dob;
      if (field.label === 'Gender') field.value = this.userData.gender;
      if (field.label === 'Language') field.value = this.userData.language;
      if (field.label === 'Currency') field.value = this.userData.currency;
    });
    
    // Exit edit mode
    section.editMode = false;
  }


  changepassword(oldpass:string , newpass:string , confirmedpass:string){
    if(newpass !== confirmedpass){
      alert("New password and confirmed password do not match");
      return;
    }

    this._PersonalInfoService.changeMyPassword(oldpass , newpass , confirmedpass).subscribe({
   
      next:(response)=>{
        console.log("Password changed successfully", response);
      },
      error:(error)=>{
        console.log("Error changing password", error);
      }
  })
  }

}