import type { Banchan } from "../types/banchan";
import { Card, CardContent } from "./ui/Card";
import { Button } from "./ui/Button";

interface BanchanListProps {
  banchans: Banchan[];
  onEdit: (banchan: Banchan) => void;
  onDelete: (id: string) => void;
}

export function BanchanList({ banchans, onEdit, onDelete }: BanchanListProps) {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("ko-KR", {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  if (banchans.length === 0) {
    return (
      <Card>
        <CardContent>
          <p className="text-center text-gray-500 py-8">
            등록된 반찬이 없습니다.
          </p>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-4">
      {banchans.map((banchan) => (
        <Card key={banchan.id}>
          <CardContent>
            <div className="flex items-center justify-between">
              <div className="flex-1">
                <h3 className="text-lg font-medium text-gray-900">
                  {banchan.name}
                </h3>
                <p className="text-sm text-gray-500">
                  등록일: {formatDate(banchan.createdAt)}
                </p>
              </div>
              <div className="flex space-x-2">
                <Button
                  variant="secondary"
                  size="sm"
                  onClick={() => onEdit(banchan)}
                >
                  수정
                </Button>
                <Button
                  variant="danger"
                  size="sm"
                  onClick={() => onDelete(banchan.id)}
                >
                  삭제
                </Button>
              </div>
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
