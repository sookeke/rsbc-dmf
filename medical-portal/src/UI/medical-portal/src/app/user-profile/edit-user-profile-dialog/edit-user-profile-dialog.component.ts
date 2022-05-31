import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { ProfileManagementService } from 'src/app/shared/services/profile.service';

export interface UserProfileDialogData{
  lastName: string;
  firstName:string;
  emailAddress:string;
}
@Component({
  selector: 'app-edit-user-profile-dialog',
  templateUrl: './edit-user-profile-dialog.component.html',
  styleUrls: ['./edit-user-profile-dialog.component.scss']
})
export class EditUserProfileDialogComponent implements OnInit{

  constructor(
    public dialogRef: MatDialogRef<EditUserProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserProfileDialogData,
    public profileService: ProfileManagementService
  ) {}
 ngOnInit(): void {
   console.log(this.data)
 }


  onCancelDialog(): void {
    this.dialogRef.close();
  }

  onConfirmChanges(){
  console.log(this.data.emailAddress)
     this.profileService.updateProfile({body: {email: this.data.emailAddress}}).subscribe(
       (success) => {
         
         console.log(success)
         this.dialogRef.close(this.data.emailAddress);
       }
     )
     
  }

}