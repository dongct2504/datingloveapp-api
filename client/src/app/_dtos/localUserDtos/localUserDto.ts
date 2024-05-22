export interface LocalUserDto {
    localUserId: string;
    userName: string;
    dateOfBirth: string;
    gender: string;
    nickname: string;
    profilePictureUrl: string | null;
    address: string | null;
    ward: string | null;
    district: string | null;
    city: string | null;
}