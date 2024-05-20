import { PictureDto } from '../pictureDtos/pictureDto'

export interface LocalUserDetailDto {
    localUserId: string;
    firstName: string | null;
    lastName: string | null;
    userName: string;
    age: number;
    gender: string;
    knownAs: string | null;
    introduction: string | null;
    interest: string | null;
    lookingFor: string | null;
    profilePictureUrl: string | null;
    lastActive: string;
    address: string | null;
    ward: string | null;
    district: string | null;
    city: string | null;
    pictures: PictureDto[] | null;
}