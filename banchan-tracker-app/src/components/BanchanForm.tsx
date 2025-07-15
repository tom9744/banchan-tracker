import { useState, useEffect } from "react";
import type {
  Banchan,
  CreateBanchanDto,
  UpdateBanchanDto,
} from "../types/banchan";
import { Card, CardContent, CardHeader, CardTitle } from "./ui/Card";
import { Input } from "./ui/Input";
import { Button } from "./ui/Button";

interface BanchanFormProps {
  banchan?: Banchan;
  onSubmit: (data: CreateBanchanDto | UpdateBanchanDto) => void;
  onCancel: () => void;
  isLoading?: boolean;
}

export function BanchanForm({
  banchan,
  onSubmit,
  onCancel,
  isLoading = false,
}: BanchanFormProps) {
  const [name, setName] = useState("");
  const [error, setError] = useState("");

  const isEditing = !!banchan;

  useEffect(() => {
    if (banchan) {
      setName(banchan.name);
    }
  }, [banchan]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    if (!name.trim()) {
      setError("반찬 이름을 입력해주세요.");
      return;
    }

    if (isEditing) {
      onSubmit({ name: name.trim() } as UpdateBanchanDto);
    } else {
      onSubmit({ name: name.trim() } as CreateBanchanDto);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>{isEditing ? "반찬 수정" : "새 반찬 추가"}</CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-4">
          <Input
            label="반찬 이름"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="예: 김치찌개"
            error={error}
            disabled={isLoading}
            autoFocus
          />

          <div className="flex space-x-3">
            <Button type="submit" disabled={isLoading} className="flex-1">
              {isLoading ? "처리중..." : isEditing ? "수정" : "추가"}
            </Button>
            <Button
              type="button"
              variant="secondary"
              onClick={onCancel}
              disabled={isLoading}
              className="flex-1"
            >
              취소
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>
  );
}
