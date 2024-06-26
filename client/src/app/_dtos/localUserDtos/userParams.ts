import { LocalUserDto } from "./localUserDto";

export class UserParams {
    pageNumber: number = 1;
    gender: string = "";
    minAge: number = 16;
    maxAge: number = 99;

    constructor(user: LocalUserDto) {
        switch (user.gender) {
            case 'male':
                this.gender = 'female';
                break;
            case 'female':
                this.gender = 'male';
                break;
            default:
                this.gender = user.gender;
        }
    }
}